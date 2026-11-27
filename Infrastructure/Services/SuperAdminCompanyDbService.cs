using Application.DTOs.Account;
using Application.DTOs.SuperAdmin;
using Application.Interfaces;
using Application.Interfaces.Database;
using Azure.Core;
using Dapper;
using Domain.CommonModels;
using Domain.Exceptions;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Options;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.Services
{
    public class SuperAdminCompanyDbService : ISuperAdminCompanyDbService
    {

        private readonly IDapperConnectionFactory _connectionFactory;
        private readonly CoreDbContext _context;
        private readonly ICompanyFolderService _companyFolderService;
        private readonly IFileStorageService _fileStorageService;
        private readonly FileUploadOptions _fileUploadOptions;

        public SuperAdminCompanyDbService(CoreDbContext context, IDapperConnectionFactory connectionFactory, ICompanyFolderService companyFolderService, IFileStorageService fileStorageService, IOptions<FileUploadOptions> fileUploadOptions)
        {
            _connectionFactory = connectionFactory;
            _context = context;
            _fileStorageService = fileStorageService;
            _companyFolderService = companyFolderService;
            _fileUploadOptions = fileUploadOptions.Value;
        }
        public async Task<PagedResult<CompanyListDto>> GetCompanyListAsync(CompanyFilters filter)
        {
            var parameters = new
            {
                SearchString = filter.SearchString?.Trim() ?? string.Empty,
                filter.Active,
                filter.PageNo,
                filter.PageSize,
                SortColumn = filter.SortColumnName?.Trim() ?? string.Empty,
                filter.SortDirection
            };

            using var connection = _connectionFactory.CreateConnection();
            using var results = await connection.QueryMultipleAsync(StoredProcudures.SuperAdmin.GetCompanyList, parameters, commandType: CommandType.StoredProcedure);

            var totalCount = await results.ReadSingleAsync<int>();
            var companies = (await results.ReadAsync<CompanyListDto>()).ToList();

            return new PagedResult<CompanyListDto>
            {
                TotalCount = totalCount,
                NextPageAvailable = (long)filter.PageNo * filter.PageSize < totalCount,
                Items = companies
            };
        }

        public async Task<int> CreateCompanyAsync(CompanyInsUpdtRequest request, long LoginUserId)
        {
            request.Company.CompanyCode = await GenerateCompanyCodeAsync(request.Company.CompanyName);

            _companyFolderService.EnsureDefaultCompanyFolders(request.Company.CompanyCode);
            if (!string.IsNullOrWhiteSpace(request.Company.CompanyLogo))
            {
                var logoFolderPath = _companyFolderService.EnsureSubFolder(request.Company.CompanyCode, CompanyFolderNames.Logo);

                var fileName = await _fileStorageService.UploadBase64FileAsync(request.Company.CompanyLogo, logoFolderPath, _fileUploadOptions.AllowedExtensions.ToArray(), _fileUploadOptions.MaxSizeMb);

                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    request.Company.CompanyLogo = _companyFolderService.GetFileUrl(request.Company.CompanyCode, CompanyFolderNames.Logo, fileName);
                }
            }

            var parameters = new
            {
                request.CompanyID,
                request.Company.CompanyCode,
                request.Company.CompanyName,
                request.Company.LegalName,
                request.Company.CompanyLogo,
                request.Company.SubTitle,
                request.Company.RegistrationNo,
                request.Company.Gstno,
                request.Company.AddressLine1,
                request.Company.AddressLine2,
                request.Company.CityName,
                request.Company.Pin,
                request.Company.StateId,
                request.Company.CountryId,
                request.Company.Phone,
                request.Company.Mobile,
                request.Company.MailId,
                request.Company.WebsiteUrl,
                request.Company.PrimaryContactName,
                request.Company.PrimaryContactPhone,
                request.Company.PrimaryContactEmail,
                request.Company.TimeZoneId,
                request.Company.BaseUtcOffsetSec,
                request.Company.CurrencyId,
                request.Company.GracePeriodDays,
                request.Company.MaxBranches,
                request.Company.MaxUsers,
                request.Company.IsTrial,
                request.Company.CompanyStatus,
                request.Company.IsActive,
                request.Company.CompanyValidFrom,
                request.Company.CompanyValidUpto,
                request.Company.FinancialYearStartMonth,
                request.Company.IsDMS,
                request.Company.IsCase,
                request.Company.IsCompliance,
                request.Company.IsTrading,
                request.Company.IsBulkMail,
                LoginUserId
            };

            using var connection = _connectionFactory.CreateConnection();
            var companyid=0;
            try
            {
                companyid = await connection.QuerySingleOrDefaultAsync<int>(StoredProcudures.SuperAdmin.CompanyInsUpdt, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex) when (ex.Number == 50000 && ex.State == 10)
            {
                throw new AppException(ex.Message, statusCode: 401);
            }
            return companyid;
        }
        public async Task<int> UpdateCompanyAsync(CompanyInsUpdtRequest request, long LoginUserId)
        {
            if (!string.IsNullOrWhiteSpace(request.Company.CompanyLogo))
            {
                var logoFolderPath = _companyFolderService.EnsureSubFolder(request.Company.CompanyName, CompanyFolderNames.Logo);

                var fileName = await _fileStorageService.UploadBase64FileAsync(request.Company.CompanyLogo, logoFolderPath, _fileUploadOptions.AllowedExtensions.ToArray(), _fileUploadOptions.MaxSizeMb);

                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    request.Company.CompanyLogo = _companyFolderService.GetFileUrl(request.Company.CompanyName, CompanyFolderNames.Logo, fileName);
                }
            }

            var parameters = new
            {
                request.CompanyID,
                request.Company.CompanyCode,
                request.Company.CompanyName,
                request.Company.LegalName,
                request.Company.CompanyLogo,
                request.Company.SubTitle,
                request.Company.RegistrationNo,
                request.Company.Gstno,
                request.Company.AddressLine1,
                request.Company.AddressLine2,
                request.Company.CityName,
                request.Company.Pin,
                request.Company.StateId,
                request.Company.CountryId,
                request.Company.Phone,
                request.Company.Mobile,
                request.Company.MailId,
                request.Company.WebsiteUrl,
                request.Company.PrimaryContactName,
                request.Company.PrimaryContactPhone,
                request.Company.PrimaryContactEmail,
                request.Company.TimeZoneId,
                request.Company.BaseUtcOffsetSec,
                request.Company.CurrencyId,
                request.Company.GracePeriodDays,
                request.Company.MaxBranches,
                request.Company.MaxUsers,
                request.Company.IsTrial,
                request.Company.CompanyStatus,
                request.Company.IsActive,
                request.Company.CompanyValidFrom,
                request.Company.CompanyValidUpto,
                request.Company.FinancialYearStartMonth,
                request.Company.IsDMS,
                request.Company.IsCase,
                request.Company.IsCompliance,
                request.Company.IsTrading,
                request.Company.IsBulkMail,
                LoginUserId
            };

            using var connection = _connectionFactory.CreateConnection();
            var companyid = 0;
            try
            {
                companyid = await connection.QuerySingleOrDefaultAsync<int>(StoredProcudures.SuperAdmin.CompanyInsUpdt, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex) when (ex.Number == 50000 && ex.State == 10)
            {
                throw new AppException(ex.Message, statusCode: 401);
            }
            return companyid;
        }
        private async Task<string> GenerateCompanyCodeAsync(string companyName)
        {
            var prefix = BuildCompanyCodePrefix(companyName);

            var counter = await _context.CompanyCodeCounters
                .FromSqlInterpolated($@"
            SELECT *
            FROM dbo.CompanyCodeCounter WITH (UPDLOCK, HOLDLOCK)
            WHERE Prefix = {prefix}").FirstOrDefaultAsync();

            int nextNumber;

            if (counter == null)
            {
                counter = new CompanyCodeCounter
                {
                    Prefix = prefix,
                    LastNumber = 1,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.CompanyCodeCounters.Add(counter);
                nextNumber = 1;
            }
            else
            {
                counter.LastNumber += 1;
                counter.UpdatedAt = DateTime.UtcNow;
                nextNumber = counter.LastNumber;
            }

            await _context.SaveChangesAsync();

            return $"{prefix}{nextNumber:D3}";
        }
        private static string BuildCompanyCodePrefix(string companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName)) throw new AppException("Company name is required.");

            var words = companyName.ToUpper().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(CleanCompanyWord).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !CompanyCodeIgnoreWords.Contains(x)).ToList();

            if (!words.Any()) throw new AppException("Valid company name is required.");

            if (words.Count == 1)
            {
                return words[0].Length >= 4 ? words[0][..4] : words[0].PadRight(4, 'X');
            }

            if (words.Count == 2)
            {
                var firstPart = words[0].Length >= 3 ? words[0][..3] : words[0];

                return (firstPart + words[1][0]).PadRight(4, 'X');
            }

            var firstWordPart = words[0].Length >= 2 ? words[0][..2] : words[0].PadRight(2, 'X');

            return $"{firstWordPart}{words[1][0]}{words[2][0]}";
        }

        private static string CleanCompanyWord(string word)
        {

            return new string(word.Where(char.IsLetterOrDigit).ToArray());
        }
    }
}
