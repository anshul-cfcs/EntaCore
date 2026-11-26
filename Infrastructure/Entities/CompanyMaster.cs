using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class CompanyMaster
{
    public int CompanyId { get; set; }

    public string CompanyCode { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public string? LegalName { get; set; }

    public string? CompanyLogo { get; set; }

    public string? SubTitle { get; set; }

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

    public int? CurrencyId { get; set; }

    public int? SubscriptionPlanId { get; set; }

    public DateTime? SubscriptionStartDate { get; set; }

    public DateTime? SubscriptionEndDate { get; set; }

    public int GracePeriodDays { get; set; }

    public int? MaxBranches { get; set; }

    public int? MaxUsers { get; set; }

    public bool IsTrial { get; set; }

    public string CompanyStatus { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsHidden { get; set; }

    public DateTime? CompanyValidFrom { get; set; }

    public DateTime? CompanyValidUpto { get; set; }

    public byte FinancialYearStartMonth { get; set; }

    public bool IsDms { get; set; }

    public bool IsCase { get; set; }

    public bool IsCompliance { get; set; }

    public bool IsTrading { get; set; }

    public bool IsBulkMail { get; set; }

    public DateTime CreatedAt { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }

    public virtual ICollection<EmployeeMaster> EmployeeMasters { get; set; } = new List<EmployeeMaster>();

    public virtual ICollection<LoginLogMaster> LoginLogMasters { get; set; } = new List<LoginLogMaster>();

    public virtual ICollection<MailConfigurationMaster> MailConfigurationMasters { get; set; } = new List<MailConfigurationMaster>();
}
