using Application.DTOs.SuperAdmin;
using Application.Interfaces.Database;
using Azure.Core;
using Dapper;
using Domain.CommonModels;
using Domain.Exceptions;
using Infrastructure.Data;
using Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class SuperAdminCompanyDbService : ISuperAdminCompanyDbService
    {

        private readonly IDapperConnectionFactory _connectionFactory;

        public SuperAdminCompanyDbService(IDapperConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<PagedResult<CompanyListDto>> GetCompanyListAsync(CompanyFilters filter)
        {
            var parameters = new
            {
                SearchString = filter.SearchString?.Trim() ?? string.Empty,
                filter.Active,
                filter.PageNo,
                filter.PageSize,
                SortColumnName = filter.SortColumnName?.Trim() ?? string.Empty,
                filter.SortDirection
            };

            var connection = _connectionFactory.CreateConnection();
            var results = await connection.QueryMultipleAsync(StoredProcudures.SuperAdmin.GetCompanyList, parameters, commandType: System.Data.CommandType.StoredProcedure);

            var totalCount = await results.ReadSingleAsync<int>();
            var companies = (await results.ReadAsync<CompanyListDto>()).ToList();

            return new PagedResult<CompanyListDto>
            {
                TotalCount = totalCount,
                NextPageAvailable = (long)filter.PageNo * filter.PageSize < totalCount,
                Items = companies
            };
        }
    }
}
