using Application.Interfaces;
using Domain.CommonModels;
using Domain.Exceptions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EntaCoreAPI.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        public TokenValidationMiddleware(RequestDelegate next, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _next = next;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? string.Empty;
            var endpoint = context.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null;

            if (allowAnonymous)
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                await WriteUnauthorized(context, "Authorization header is missing.");
                return;
            }

            var bearerValue = authorizationHeader.ToString();

            if (string.IsNullOrWhiteSpace(bearerValue) ||
                !bearerValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                await WriteUnauthorized(context, "Authorization header is invalid.");
                return;
            }

            var encryptedToken = bearerValue["Bearer ".Length..].Trim();

            if (string.IsNullOrWhiteSpace(encryptedToken))
            {
                await WriteUnauthorized(context, "Token not found.");
                return;
            }

            ClaimsPrincipal principal;
            long userLoginId;
            string? employeeIdValue;
            string? companyIdValue;
            string? branchIdValue;
            string? roleIdValue;
            string? roleCodeValue;
            string? userName;
            string decryptedToken;

            try
            {
                using var scope = _serviceProvider.CreateScope();

                var encryptionService = scope.ServiceProvider.GetRequiredService<IEncryptionService>();
                var dbContext = scope.ServiceProvider.GetRequiredService<CoreDbContext>();

                try
                {
                    decryptedToken = encryptionService.DecryptFromString(encryptedToken);
                }
                catch
                {
                    await WriteUnauthorized(context, "Token decryption failed.");
                    return;
                }

                var validatedPrincipal = ValidateJwtToken(decryptedToken);

                if (validatedPrincipal == null)
                {
                    await WriteUnauthorized(context, "Invalid token.");
                    return;
                }

                principal = validatedPrincipal;

                var userLoginIdValue = principal.FindFirst("UserLoginID")?.Value ?? principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                employeeIdValue = principal.FindFirst("EmployeeID")?.Value;
                companyIdValue = principal.FindFirst("CompanyID")?.Value;
                branchIdValue = principal.FindFirst("BranchID")?.Value;
                roleIdValue = principal.FindFirst("RoleID")?.Value;
                roleCodeValue = principal.FindFirst("RoleCode")?.Value;

                var authCode = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                userName = principal.FindFirst("unique_name")?.Value ?? principal.FindFirst("upn")?.Value ?? principal.FindFirst(ClaimTypes.Name)?.Value ?? principal.FindFirst(ClaimTypes.Upn)?.Value;

                if (!long.TryParse(userLoginIdValue, out userLoginId) || userLoginId <= 0)
                {
                    await WriteUnauthorized(context, "UserLoginID claim is missing or invalid.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(authCode))
                {
                    await WriteUnauthorized(context, "Token session id is missing.");
                    return;
                }

                var hashedTokenBytes = encryptionService.GenerateAuthTokenHash(authCode);

                var userLogin = await dbContext.UserLogins.AsNoTracking().FirstOrDefaultAsync(x =>
                        x.UserLoginId == userLoginId &&
                        x.IsActive &&
                        x.LoginActive &&
                        !x.IsDeleted);

                if (userLogin == null)
                {
                    await WriteUnauthorized(context, "User session is invalid.");
                    return;
                }

                if (userLogin.CurrentToken == null || !userLogin.CurrentToken.SequenceEqual(hashedTokenBytes))
                {
                    await WriteUnauthorized(context, "Token is invalid or user has logged out.");
                    return;
                }
            }
            catch (AppException)
            {
                throw;
            }
            catch
            {
                await WriteUnauthorized(context, "Unauthorized access.");
                return;
            }

            context.User = principal;

            context.Items["UserLoginID"] = userLoginId;
            context.Items["EmployeeID"] = employeeIdValue;
            context.Items["CompanyID"] = companyIdValue;
            context.Items["BranchID"] = branchIdValue;
            context.Items["RoleID"] = roleIdValue;
            context.Items["RoleCode"] = roleCodeValue;
            context.Items["UserName"] = userName;
            context.Items["Authorization"] = decryptedToken;

            await _next(context);
        }
        private ClaimsPrincipal? ValidateJwtToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(jwtToken))
                return null;

            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(jwtKey) || string.IsNullOrWhiteSpace(jwtIssuer) || string.IsNullOrWhiteSpace(jwtAudience))
            {
                return null;
            }

            var key = Convert.FromBase64String(jwtKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                ValidateAudience = true,
                ValidAudience = jwtAudience,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                return tokenHandler.ValidateToken(jwtToken, validationParameters, out _);
            }
            catch
            {
                return null;
            }
        }
        private static async Task WriteUnauthorized(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new ResponseJson(new Meta(StatusCodes.Status401Unauthorized, message));

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
