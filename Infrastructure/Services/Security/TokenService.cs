using Application.DTOs.Account;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services.Security
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IEncryptionService _encryptionService;
        public TokenService(IConfiguration configuration, IEncryptionService encryptionService)
        {
            _configuration = configuration;
            _encryptionService = encryptionService;
        }
        public (string encryptedToken, string tokenExpiryTime) CreateToken(LoginTokenDetail login)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, login.UserLoginID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, login.AuthCode),
                new Claim("UserLoginID", login.UserLoginID.ToString()),
                new Claim("EmployeeID", login.EmployeeID.ToString()),
                new Claim("CompanyID", login.CompanyID.ToString()),
                new Claim("RoleID", login.RoleID.ToString()),
                new Claim("RoleCode", login.RoleCode),
                new Claim("unique_name", login.UserName),
                new Claim("upn", login.UserName),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            if (login.ZoneID.HasValue)
            {
                claims.Add(new Claim("ZoneID", login.ZoneID.Value.ToString()));
            }
            if (login.BranchID.HasValue)
            {
                claims.Add(new Claim("BranchID", login.BranchID.Value.ToString()));
            }

            var secretKey = Convert.FromBase64String(_configuration["Jwt:Key"]!);
            var securityKey = new SymmetricSecurityKey(secretKey);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
            var expirationValue = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:Expiration_Minutes"]));

            var tokenGenerator = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience : _configuration["Jwt:Audience"],
                claims : claims,
                expires : expirationValue,
                signingCredentials : signingCredentials
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenGenerator);

            var jwtToken = tokenHandler.ReadJwtToken(token);
            var tokenExpiryTime = ((DateTimeOffset)jwtToken.ValidTo).ToUnixTimeMilliseconds();

            return (_encryptionService.EncryptToString(token), tokenExpiryTime.ToString());
        }
    }
}
