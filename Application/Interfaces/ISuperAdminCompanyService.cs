using Application.DTOs.SuperAdmin;
using Domain.CommonModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ISuperAdminCompanyService
    {
        Task<PagingMeta> GetCompanyListAsync(CompanyFilters filter);
        Task<PagingMeta> CreateCompanyAsync(CompanyInsUpdtRequest request);
    }
}
