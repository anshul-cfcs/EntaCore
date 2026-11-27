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
            public const string CompanyInsUpdt = "dbo.CompanyInsUpdt";
        }
        public static class Account
        {
            public const string UserLoginCheck = "dbo.UserLoginCheck";
            public const string UserLoginOTPCheck = "dbo.UserLoginOTPCheck";
            public const string UserForgotPassword = "dbo.UserForgotPassword";
            public const string UserResetPassword = "dbo.UserResetPassword";
            public const string UserPasswordUpdate = "dbo.UserPasswordUpdate";
            public const string LoginOTPRegenerate = "dbo.LoginOTPRegenerate";
        }
    }
}
