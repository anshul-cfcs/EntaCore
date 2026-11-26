using Application.DTOs.Account;

namespace Application.Interfaces.Database
{
    public interface IAuthenticateDbService
    {
        Task<LoginResultDto?> LoginAsync(LoginRequest request);
    }
}
