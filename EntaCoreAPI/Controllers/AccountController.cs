using Application.DTOs.Account;
using Application.Interfaces;
using Application.Services;
using Domain.CommonModels;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntaCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                return Ok(await _accountService.Login(request));
            }
            catch (AppException ex) when (ex.StatusCode == StatusCodes.Status401Unauthorized)
            {
                return Unauthorized(new ResponseJson(new Meta(ex.StatusCode, ex.Message)));
            }
        }

        [AllowAnonymous]
        [HttpPost("verify-login-otp")]
        public async Task<IActionResult> VerifyLoginOtp([FromBody] VerifyLoginOTPRequest request)
        {
            try
            {
                return Ok(await _accountService.VerifyLoginOtp(request));
            }
            catch (AppException ex) when (ex.StatusCode == StatusCodes.Status401Unauthorized)
            {
                return Unauthorized(new ResponseJson(new Meta(ex.StatusCode, ex.Message)));
            }
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                return Ok(await _accountService.ForgotPassword(request));
            }
            catch (AppException ex) when (ex.StatusCode == StatusCodes.Status401Unauthorized)
            {
                return Unauthorized(new ResponseJson(new Meta(ex.StatusCode, ex.Message)));
            }
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ChangePassword([FromHeader] string Authorization, [FromBody] ChangePasswordRequest request)
        {
            try
            {
                return Ok(await _accountService.ChangePassword(request));
            }
            catch (AppException ex) when (ex.StatusCode == StatusCodes.Status401Unauthorized)
            {
                return Unauthorized(new ResponseJson(new Meta(ex.StatusCode, ex.Message)));
            }
        }

        [AllowAnonymous]
        [HttpPost("login-otp-regenerate")]
        public async Task<IActionResult> LoginOtpRegenerate([FromBody] LoginOTPRegenerateRequest request)
        {
            try
            {
                return Ok(await _accountService.LoginOtpRegenerate(request));
            }
            catch (AppException ex) when (ex.StatusCode == StatusCodes.Status401Unauthorized)
            {
                return Unauthorized(new ResponseJson(new Meta(ex.StatusCode, ex.Message)));
            }
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromHeader] string Authorization)
        {
            try
            {
                return Ok(await _accountService.LogoutAsync());
            }
            catch (AppException ex) when (ex.StatusCode == StatusCodes.Status401Unauthorized)
            {
                return Unauthorized(new ResponseJson(new Meta(ex.StatusCode, ex.Message)));
            }
        }
    }
}
