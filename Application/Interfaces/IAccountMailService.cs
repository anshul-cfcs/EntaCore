using Application.DTOs.Account;

namespace Application.Interfaces
{
    public interface IAccountMailService
    {
        Task SendLoginOtpAsync(LoginResultDto result);
        Task SendForgotPasswordOtpAsync(LoginResultDto result);
        Task SendPasswordChangedAsync(LoginResultDto result);
    }
}
