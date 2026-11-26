using Application.DTOs.Account;
using Application.Interfaces;
using Application.Interfaces.Database;
using Domain.CommonModels;
using Domain.Exceptions;

namespace Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITokenService _tokenService;
        private readonly IEncryptionService _encryptionService;
        private readonly IAuthenticateDbService _authenticationDbService;
        private readonly IAccountMailService _accountMailService;

        public AccountService(IAuthenticateDbService authenticationDbService, ITokenService tokenService, IEncryptionService encryptionService, IAccountMailService accountMailService)
        {
            _tokenService = tokenService;
            _encryptionService = encryptionService;
            _authenticationDbService = authenticationDbService;
            _accountMailService = accountMailService;
        }
        public async Task<ResponseJson> Login(LoginRequest request)
        {
            var result = await _authenticationDbService.LoginAsync(request);

            if (result == null || result is null) throw new AppException("Invalid username or password.", statusCode: 401);

            var authCode = result.AuthCode;

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
                PasswordChangeReminderDays = result.PasswordChangeReminderDays
            };

            return new ResponseJson(new Meta(1, "Success"), response);
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
    }
}
