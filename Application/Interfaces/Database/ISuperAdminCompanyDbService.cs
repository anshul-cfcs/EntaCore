using Application.DTOs.SuperAdmin;
using Domain.CommonModels;

namespace Application.Interfaces.Database
{
    public interface ISuperAdminCompanyDbService
    {
        Task<PagedResult<CompanyListDto>> GetCompanyListAsync(CompanyFilters filter);
        Task<int> CreateCompanyAsync(CompanyInsUpdtRequest request, long LoginUserId);
        Task<int> UpdateCompanyAsync(CompanyInsUpdtRequest request, long LoginUserId);
    }
}
