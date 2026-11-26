using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    internal class TokenDetailDto
    {
        public string Token { get; set; } = string.Empty;
        public string ExpiryTime { get; set; } = string.Empty;
        public long UserLoginID { get; set; }
        public Guid EmployeeID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string LoginName { get; set; } = string.Empty;
        public int CompanyID { get; set; }
        public int RoleID { get; set; }
        public int? BranchID { get; set; }
        public int? ZoneID { get; set; }
        public int PasswordAge { get; set; }
        public int PasswordChangeReminderDays { get; set; }
        public bool SubscriptionExpired { get; set; }
        public bool RedirectToRenewal { get; set; }
        public bool IsDMS { get; set; }
        public bool IsCompliance { get; set; }
        public bool IsCase { get; set; }
        public bool IsTrade { get; set; }
        public bool IsBulkMail { get; set; }
    }
}
