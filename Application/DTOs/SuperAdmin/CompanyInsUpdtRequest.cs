using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.SuperAdmin
{
    public class CompanyInsUpdtRequest
    {
        public int? CompanyID { get; set; }

        public CompanyInfoDto Company { get; set; } = new();
        public SubscriptionInfoDto? Subscription { get; set; }
    }
    public class CompanyInfoDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string? LegalName { get; set; }
        public string? CompanyLogo { get; set; }
        public string? SubTitle { get; set; }

        public string? CompanyCode { get; set; }

        public string? RegistrationNo { get; set; }
        public string? Gstno { get; set; }

        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? CityName { get; set; }
        public string? Pin { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }

        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? MailId { get; set; }

        public string? WebsiteUrl { get; set; }
        public string? PrimaryContactName { get; set; }
        public string? PrimaryContactPhone { get; set; }
        public string? PrimaryContactEmail { get; set; }

        public int? TimeZoneId { get; set; }
        public int? BaseUtcOffsetSec { get; set; }
        public byte FinancialYearStartMonth { get; set; }
        public int? CurrencyId { get; set; }

        public bool IsTrial { get; set; }
        public string CompanyStatus { get; set; } = "ACTIVE";

        public DateTime? CompanyValidFrom { get; set; }
        public DateTime? CompanyValidUpto { get; set; }

        public int GracePeriodDays { get; set; }
        public int? MaxBranches { get; set; }
        public int? MaxUsers { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDMS { get; set; } = true;
        public bool IsCase { get; set; } = true;
        public bool IsCompliance { get; set; } = true;
        public bool IsTrading { get; set; } = true;
        public bool IsBulkMail { get; set; } = true;
    }

    public class SubscriptionInfoDto
    {
        public int SubscriptionPlanId { get; set; }
        public DateTime SubscriptionStartDate { get; set; }
        
    }
}
