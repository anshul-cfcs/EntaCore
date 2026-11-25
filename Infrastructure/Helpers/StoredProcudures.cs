using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Helpers
{
    public static class StoredProcudures
    {
        public static class SuperAdmin
        {
            public const string GetCompanyList = "dbo.CompanyList";
            public const string GetCompanyDetail = "dbo.CompanyDetail";
        }
    }
}
