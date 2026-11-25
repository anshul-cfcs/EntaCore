using Application.DTOs.SuperAdmin;
using Application.Interfaces;
using Application.Interfaces.Database;
using Domain.CommonModels;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;

namespace Application.Services
{
    public class SuperAdminCompanyService : ISuperAdminCompanyService
    {
        private readonly ISuperAdminCompanyDbService _dbService;
        public SuperAdminCompanyService(ISuperAdminCompanyDbService dbService)
        {
            _dbService = dbService;
        }
        public async Task<PagingMeta> GetCompanyListAsync(CompanyFilters filter)
        {
            filter.PageNo = Math.Max(filter.PageNo, 1);
            filter.PageSize = filter.PageSize > 0 ? filter.PageSize : 10;

            if (filter.Active is not (0 or 1 or 2)) throw new AppException("Invalid active filter.");

            var result = await _dbService.GetCompanyListAsync(filter);

            return new PagingMeta(new Meta(1, result.Items.Count > 0 ? "Success" : "Data Not Found"), new data(filter.PageNo, result.TotalCount, result.NextPageAvailable, result.Items));
        }
    }
}
