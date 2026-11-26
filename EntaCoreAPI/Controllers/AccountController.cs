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
    }
}
