using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.SuperAdmin
{
    public class CompanyListDto
    {
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string? CompanyLogo { get; set; }
        public string? MailId { get; set; }
        public string? Mobile { get; set; }
        public string? CityName { get; set; }
        public int? CurrencyId { get; set; }
        public string? CurrencyName { get; set; }

        public int? SubscriptionPlanId { get; set; }
        public string? SubscriptionPlanName { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public int GracePeriodDays { get; set; }

        public int? MaxBranches { get; set; }
        public int? MaxUsers { get; set; }

        public bool IsTrial { get; set; }
        public DateTime? CompanyValidFrom { get; set; }
        public DateTime? CompanyValidUpto { get; set; }
        public string CompanyStatus { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
