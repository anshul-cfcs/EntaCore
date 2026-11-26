using Application.DTOs.Account;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        public (string encryptedToken, string tokenExpiryTime) CreateToken(LoginTokenDetail login);
    }
}
