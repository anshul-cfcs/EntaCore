using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class LoginResultDto
    {
        public long UserLoginID { get; set; }
        public Guid EmployeeID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string EmailID { get; set; } = string.Empty;
        public int CompanyID { get; set; }
        public string RoleCode { get; set; } = string.Empty;
        public int RoleID { get; set; }
        public string AuthCode { get; set; } = string.Empty;
        public bool IsTwoFactorAuth { get; set; }
        public string? OTP { get; set; }
        public Guid? OTPKey { get; set; }

        public int PasswordAge { get; set; }
        public int PasswordChangeReminderDays { get; set; }
    }
}
