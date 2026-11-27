using Application.DTOs.Account;

namespace Application.Interfaces.Database
{
    public interface IAuthenticateDbService
    {
        Task<LoginResultDto?> LoginAsync(LoginRequest request);
        Task<VerifyLoginOTPResultDto?> VerifyLoginOtpAsync(VerifyLoginOTPRequest request);
        Task<LoginResultDto?> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<LoginResultDto?> ResetPasswordAsync(ResetPasswordRequest request);
        Task<LoginResultDto?> ChangePasswordAsync(long userLoginId, ChangePasswordRequest request);
        Task<LoginResultDto?> LoginOtpRegenerateAsync(LoginOTPRegenerateRequest request);
        Task<bool> LogoutAsync(long userLoginId, byte[] hashedToken);
        Task<LoginCompanyAccessDto?> GetLoginCompanyAccessAsync(int companyId);
    }
}
