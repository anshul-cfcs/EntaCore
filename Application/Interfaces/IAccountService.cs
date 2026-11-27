using Application.DTOs.Account;
using Domain.CommonModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseJson> Login(LoginRequest request);
        Task<ResponseJson> VerifyLoginOtp(VerifyLoginOTPRequest request);
        Task<ResponseJson> ForgotPassword(ForgotPasswordRequest request);
        Task<ResponseJson> ResetPassword(ResetPasswordRequest request);
        Task<ResponseJson> ChangePassword(ChangePasswordRequest request);
        Task<ResponseJson> LoginOtpRegenerate(LoginOTPRegenerateRequest request);
        Task<ResponseJson> LogoutAsync();
    }
}
