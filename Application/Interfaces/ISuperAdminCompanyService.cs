using Application.DTOs.SuperAdmin;
using Domain.CommonModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ISuperAdminCompanyService
    {
        Task<PagingMeta> GetCompanyListAsync(CompanyFilters filter);
    }
}
