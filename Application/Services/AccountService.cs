using Application.DTOs.Account;
using Application.Interfaces;
using Application.Interfaces.Database;
using Domain.CommonModels;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITokenService _tokenService;
        private readonly IEncryptionService _encryptionService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuthenticateDbService _authenticationDbService;
        private readonly IAccountMailService _accountMailService;

        public AccountService(IAuthenticateDbService authenticationDbService, ITokenService tokenService, IEncryptionService encryptionService, IAccountMailService accountMailService, ICurrentUserService currentUserService)
        {
            _tokenService = tokenService;
            _encryptionService = encryptionService;
            _authenticationDbService = authenticationDbService;
            _accountMailService = accountMailService;
            _currentUserService = currentUserService;
        }
        
        
        public async Task<ResponseJson> Login(LoginRequest request)
        {
            var result = await _authenticationDbService.LoginAsync(request);

            if (result == null || result is null) throw new AppException("Invalid username or password.", statusCode: 401);

            var authCode = result.AuthCode;

            var access = await ValidateCompanyAccessAsync(result.CompanyID, result.RoleID);

            if (string.IsNullOrWhiteSpace(authCode))
                throw new InvalidOperationException("Login did not return a session ID.");

            // If OTP flow is enabled
            if (result.OTPKey.HasValue)
            {
                await _accountMailService.SendLoginOtpAsync(result);

                return new ResponseJson(
                    new Meta(1, $"OTP sent on {MaskEmail(result.EmailID)}, please verify to continue."),
                    new
                    {
                        IsAuthValue = true,
                        OTPKey = result.OTPKey
                    });
            }

            var tokenPayload = new LoginTokenDetail
            {
                UserLoginID = result.UserLoginID,
                EmployeeID = result.EmployeeID,
                CompanyID = result.CompanyID,
                RoleID = result.RoleID,
                RoleCode = result.RoleCode,
                UserName = result.UserName,
                AuthCode = authCode
            };

            var tokenResult = _tokenService.CreateToken(tokenPayload);

            var response = new TokenDetailDto
            {
                Token = tokenResult.encryptedToken,
                ExpiryTime = tokenResult.tokenExpiryTime,
                UserLoginID = result.UserLoginID,
                EmployeeID = result.EmployeeID,
                UserName = result.UserName,
                LoginName = result.FullName,
                CompanyID = result.CompanyID,
                RoleID = result.RoleID,
                PasswordAge = result.PasswordAge,
                PasswordChangeReminderDays = result.PasswordChangeReminderDays,
                SubscriptionExpired = access.SubscriptionExpired,
                RedirectToRenewal = access.RedirectToRenewal
            };

            return new ResponseJson(new Meta(1, "Success"), response);
        }
        public async Task<ResponseJson> VerifyLoginOtp(VerifyLoginOTPRequest request)
        {
            var result = await _authenticationDbService.VerifyLoginOtpAsync(request);

            if (result == null)
                throw new AppException("Invalid OTP or OTP expired");

            var access = await ValidateCompanyAccessAsync(result.CompanyID, result.RoleID);

            var tokenPayload = new LoginTokenDetail
            {
                UserLoginID = result.UserLoginID,
                EmployeeID = result.EmployeeID,
                CompanyID = result.CompanyID,
                BranchID = result.BranchID,
                RoleID = result.RoleID,
                RoleCode = result.RoleCode,
                UserName = result.UserName,
                AuthCode = result.AuthCode ?? Guid.NewGuid().ToString("N")
            };

            var tokenResult = _tokenService.CreateToken(tokenPayload);
            //var hierarchy = await _userRoleDbService.GetEmployeeHierarchyAsync(result.CompanyID, result.EmployeeID);

            var response = new TokenDetailDto
            {
                Token = tokenResult.encryptedToken,
                ExpiryTime = tokenResult.tokenExpiryTime,
                UserLoginID = result.UserLoginID,
                EmployeeID = result.EmployeeID,
                UserName = result.UserName,
                LoginName = result.FullName,
                CompanyID = result.CompanyID,
                BranchID = result.BranchID,
                RoleID = result.RoleID,
                PasswordAge = result.PasswordAge,
                PasswordChangeReminderDays = result.PasswordChangeReminderDays,
                SubscriptionExpired = access.SubscriptionExpired,
                RedirectToRenewal = access.RedirectToRenewal
                //IsHR = result.RoleID == (int)LoginType.CompanyAdmin || result.RoleID == (int)LoginType.SuperAdmin,
                //IsRM = hierarchy.IsRM,
                //IsCH = hierarchy.IsCH
            };

            return new ResponseJson(new Meta(1, "Success"), response);
        }
        public async Task<ResponseJson> ForgotPassword(ForgotPasswordRequest request)
        {
            var result = await _authenticationDbService.ForgotPasswordAsync(request);

            if (result == null)
                return new ResponseJson(new Meta(0, "Failed to process forgot password"));

            await _accountMailService.SendForgotPasswordOtpAsync(result);

            return new ResponseJson(new Meta(1, $"Please check your email {MaskEmail(result.EmailID)} and proceed as instructed."),
                new { EmailID = result.EmailID });
        }
        public async Task<ResponseJson> ResetPassword(ResetPasswordRequest request)
        {
            var result = await _authenticationDbService.ResetPasswordAsync(request);

            if (result == null)
                return new ResponseJson(new Meta(0, "Failed to reset password"));

            await _accountMailService.SendPasswordChangedAsync(result);

            return new ResponseJson(new Meta(1, "Password updated successfully"));
        }
        public async Task<ResponseJson> ChangePassword(ChangePasswordRequest request)
        {
            var userLoginId = _currentUserService.GetUserLoginId();
            var result = await _authenticationDbService.ChangePasswordAsync(userLoginId, request);

            if (result == null)
                throw new AppException("Failed to change password");

            await _accountMailService.SendPasswordChangedAsync(result);

            return new ResponseJson(new Meta(1, "Password updated successfully"));
        }
        public async Task<ResponseJson> LoginOtpRegenerate(LoginOTPRegenerateRequest request)
        {
            var result = await _authenticationDbService.LoginOtpRegenerateAsync(request);

            if (result == null)
                return new ResponseJson(new Meta(0, "Failed to regenerate OTP"));

            await _accountMailService.SendLoginOtpAsync(result);

            return new ResponseJson(new Meta(1, $"OTP sent on {MaskEmail(result.EmailID)}. Please verify to continue."),
                new
                {
                    IsAuthValue = true,
                    OTPKey = result.OTPKey
                });
        }
        public async Task<ResponseJson> LogoutAsync()
        {
            var userLoginId = _currentUserService.GetUserLoginId();
            var authCode = _currentUserService.GetAuthCode();

            if (userLoginId <= 0) throw new AppException("Invalid user session.", 401);

            if (string.IsNullOrWhiteSpace(authCode)) throw new AppException("Invalid token session.", 401);

            var hashedToken = _encryptionService.GenerateAuthTokenHash(authCode);

            var isLoggedOut = await _authenticationDbService.LogoutAsync(userLoginId, hashedToken);

            if (!isLoggedOut) throw new AppException("User session not found.", 401);

            return new ResponseJson(new Meta(1, "Logout successfully"));
        }
        private async Task<LoginAccessResult> ValidateCompanyAccessAsync(int companyId, int roleId)
        {
            if (roleId == (int)LoginType.SuperAdmin)
                return new LoginAccessResult();

            var companyAccess = await _authenticationDbService.GetLoginCompanyAccessAsync(companyId);
            var today = DateTime.Today;

            if (companyAccess == null || companyAccess.IsDeleted || !companyAccess.IsActive) 
                throw new AppException("Company is inactive. Please contact administrator.");

            if (companyAccess.CompanyValidFrom.HasValue && companyAccess.CompanyValidFrom.Value.Date > today) 
                throw new AppException("Company registration is not active. Please contact administrator.");

            if (companyAccess.CompanyValidUpto.HasValue && companyAccess.CompanyValidUpto.Value.Date < today)
                throw new AppException("Company registration has expired. Please contact administrator.");

            var subscriptionExpired = companyAccess.SubscriptionEndDate.HasValue && companyAccess.SubscriptionEndDate.Value.Date.AddDays(companyAccess.GracePeriodDays) < today;

            return new LoginAccessResult
            {
                SubscriptionExpired = subscriptionExpired,
                RedirectToRenewal = subscriptionExpired
            };
        }
        private static string MaskEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return email;
            var atIndex = email.IndexOf('@');
            if (atIndex <= 2) return email;

            var localPart = email[..atIndex];
            var domainPart = email[atIndex..];

            var maskedLocal = localPart.Length <= 4 ? localPart[..1] + new string('*', Math.Max(localPart.Length - 1, 0)) : localPart[..2] + new string('*', localPart.Length - 4) + localPart[^2..];

            return maskedLocal + domainPart;
        }
        private sealed class LoginAccessResult
        {
            public bool SubscriptionExpired { get; init; }
            public bool RedirectToRenewal { get; init; }
        }
    }
}
