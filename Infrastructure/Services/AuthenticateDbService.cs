using Application.DTOs.Account;
using Application.Interfaces.Database;
using Dapper;
using Domain.Exceptions;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace Infrastructure.Services
{
    public class AuthenticateDbService : IAuthenticateDbService
    {
        private readonly IDapperConnectionFactory _connectionFactory;
        public AuthenticateDbService(CoreDbContext context, IDapperConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<LoginResultDto?> LoginAsync(LoginRequest request)
        {
            var parameters = new
            {
                request.UserName,
                request.Password,
                request.UserSalt,
                ClientName = request.ClientName ?? string.Empty,
                ClientVersion = request.ClientVersion ?? string.Empty,
                Platform = request.Platform ?? string.Empty,
                UserIPAddress = request.ClientIP ?? string.Empty
            };

            using var connection = _connectionFactory.CreateConnection();
            try
            {
                return await connection.QuerySingleOrDefaultAsync<LoginResultDto>(StoredProcudures.Account.UserLoginCheck, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex) when (ex.Number == 50000 && ex.State == 10)
            {
                throw new AppException("Invalid username or password.", statusCode: 401);
            }
        }
    }
}
