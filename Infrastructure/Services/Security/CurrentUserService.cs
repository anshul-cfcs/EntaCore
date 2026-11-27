using Application.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services.Security
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public long GetUserLoginId() => long.TryParse(User?.FindFirst("UserLoginID")?.Value, out var value) ? value : 0;

        public Guid GetEmployeeId() => Guid.TryParse(User?.FindFirst("EmployeeID")?.Value, out var value) ? value : Guid.Empty;

        public int GetCompanyId() => int.TryParse(User?.FindFirst("CompanyID")?.Value, out var value) ? value : 0;

        public int? GetBranchId() => int.TryParse(User?.FindFirst("BranchID")?.Value, out var value) ? value : null;
        public int? GetZoneId() => int.TryParse(User?.FindFirst("ZoneID")?.Value, out var value) ? value : null;

        public int GetRoleId() => int.TryParse(User?.FindFirst("RoleID")?.Value, out var value) ? value : 0;

        public string? GetRoleCode() => User?.FindFirst("RoleCode")?.Value;

        public string? GetUserName() => User?.FindFirst("unique_name")?.Value ?? User?.FindFirst("upn")?.Value;
        public string? GetAuthCode()
        {
            return User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        }

        public int GetAllowedCompanyId(int requestedCompanyId)
        {
            var loginUserRole = GetRoleId();

            if (loginUserRole == (int)LoginType.SuperAdmin) return requestedCompanyId;
            var loginCompanyId = GetCompanyId();
            if (loginCompanyId <= 0) throw new AppException("Company not found for user.");
            if (loginCompanyId != requestedCompanyId) throw new AppException("You are not authorized to access this.", StatusCodes.Status403Forbidden);
            return loginCompanyId;
        }
    }
}
