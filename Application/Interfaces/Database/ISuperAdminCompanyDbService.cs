using Application.DTOs.SuperAdmin;
using Domain.CommonModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Database
{
    public interface ISuperAdminCompanyDbService
    {
        Task<PagedResult<CompanyListDto>> GetCompanyListAsync(CompanyFilters filter);
    }
}
