using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class VerifyLoginOTPResultDto
    {
        public long UserLoginID { get; set; }
        public Guid EmployeeID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string EmailID { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public int CompanyID { get; set; }
        public int? BranchID { get; set; }

        public int RoleID { get; set; }
        public string RoleCode { get; set; } = string.Empty;
        public string? AuthCode { get; set; }

        public int PasswordAge { get; set; }
        public int PasswordChangeReminderDays { get; set; }
    }
}
