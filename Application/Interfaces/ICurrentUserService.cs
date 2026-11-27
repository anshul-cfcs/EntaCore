using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ICurrentUserService
    {
        long GetUserLoginId();
        Guid GetEmployeeId();
        int GetCompanyId();
        int? GetBranchId();
        int? GetZoneId();
        int GetRoleId();
        string? GetRoleCode();
        string? GetUserName();
        string? GetAuthCode();
        int GetAllowedCompanyId(int requestedCompanyId);
    }
}
