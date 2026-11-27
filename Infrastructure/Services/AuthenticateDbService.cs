using Application.DTOs.Account;
using Application.Interfaces.Database;
using Dapper;
using Domain.Exceptions;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Services
{
    public class AuthenticateDbService : IAuthenticateDbService
    {
        private readonly IDapperConnectionFactory _connectionFactory;
        private readonly CoreDbContext _context;
        public AuthenticateDbService(CoreDbContext context, IDapperConnectionFactory connectionFactory)
        {
            _context = context;
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
                throw new AppException(ex.Message, statusCode: 401);
            }
        }
        public async Task<VerifyLoginOTPResultDto?> VerifyLoginOtpAsync(VerifyLoginOTPRequest request)
        {
            var parameters = new
            {
                request.OTPKey,
                request.OTP,
                request.UserSalt,
                ClientName = request.ClientName ?? string.Empty,
                ClientVersion = request.ClientVersion ?? string.Empty,
                Platform = request.Platform ?? string.Empty,
                UserIPAddress = request.ClientIP ?? string.Empty
            };

            using var connection = _connectionFactory.CreateConnection();
            try
            {
                return await connection.QuerySingleOrDefaultAsync<VerifyLoginOTPResultDto>(StoredProcudures.Account.UserLoginOTPCheck, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex) when (ex.Number == 50000 && ex.State == 10)
            {
                throw new AppException(ex.Message, statusCode: 401);
            }
        }
        public async Task<LoginResultDto?> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var parameters = new
            {
                request.UserName
            };
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                return await connection.QuerySingleOrDefaultAsync<LoginResultDto>(StoredProcudures.Account.UserForgotPassword, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex) when (ex.Number == 50000 && ex.State == 10)
            {
                throw new AppException(ex.Message, statusCode: 401);
            }
        }
        public async Task<LoginResultDto?> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var parameters = new
            {
                request.ResetSalt,
                request.OTP,
                request.PasswordHash
            };
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                return await connection.QuerySingleOrDefaultAsync<LoginResultDto>(StoredProcudures.Account.UserResetPassword, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex) when (ex.Number == 50000 && ex.State == 10)
            {
                throw new AppException(ex.Message, statusCode: 401);
            }

        }
        public async Task<LoginResultDto?> ChangePasswordAsync(long userLoginId, ChangePasswordRequest request)
        {
            var parameters = new
            {
                UserLoginID = userLoginId,
                request.PasswordHash
            };
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                return await connection.QuerySingleOrDefaultAsync<LoginResultDto>(StoredProcudures.Account.UserPasswordUpdate, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex) when (ex.Number == 50000 && ex.State == 10)
            {
                throw new AppException(ex.Message, statusCode: 401);
            }
        }
        public async Task<LoginResultDto?> LoginOtpRegenerateAsync(LoginOTPRegenerateRequest request)
        {
            var parameters = new
            {
                request.OTPKey
            };
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                return await connection.QuerySingleOrDefaultAsync<LoginResultDto>(StoredProcudures.Account.LoginOTPRegenerate, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex) when (ex.Number == 50000 && ex.State == 10)
            {
                throw new AppException(ex.Message, statusCode: 401);
            }
        }
        public async Task<bool> LogoutAsync(long userLoginId, byte[] hashedToken)
        {
            var userLogin = await _context.UserLogins.FirstOrDefaultAsync(x => x.UserLoginId == userLoginId && x.IsActive && x.LoginActive && !x.IsDeleted);

            if (userLogin == null) return false;

            userLogin.CurrentToken = null;
            userLogin.UpdatedAt = DateTime.UtcNow;

            var loginLog = await _context.LoginLogMasters.Where(x => x.UserLoginId == userLoginId && x.LogOutTime == null && x.SessionId != null).OrderByDescending(x => x.LoginTime).FirstOrDefaultAsync();

            if (loginLog != null && loginLog.SessionId!.SequenceEqual(hashedToken))
            {
                loginLog.LogOutTime = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<LoginCompanyAccessDto?> GetLoginCompanyAccessAsync(int companyId)
        {
            return await _context.CompanyMasters.AsNoTracking().Where(x => x.CompanyId == companyId)
            .Select(x => new LoginCompanyAccessDto
            {
                CompanyId = x.CompanyId,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted,
                CompanyValidFrom = x.CompanyValidFrom,
                CompanyValidUpto = x.CompanyValidUpto,
                SubscriptionStartDate = x.SubscriptionStartDate,
                SubscriptionEndDate = x.SubscriptionEndDate,
                GracePeriodDays = x.GracePeriodDays
            })
            .FirstOrDefaultAsync();
        }
    }
}
