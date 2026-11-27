using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class LoginCompanyAccessDto
    {
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSystemCompany { get; set; }
        public DateTime? CompanyValidFrom { get; set; }
        public DateTime? CompanyValidUpto { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public int GracePeriodDays { get; set; }
    }
}
