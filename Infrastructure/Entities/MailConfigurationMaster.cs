using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class MailConfigurationMaster
{
    public Guid MailConfigurationId { get; set; }

    public int? CompanyId { get; set; }

    public string SenderMailId { get; set; } = null!;

    public string SenderPasswordHash { get; set; } = null!;

    public string Smtpserver { get; set; } = null!;

    public int Smtpport { get; set; }

    public bool SslMode { get; set; }

    public string ServiceName { get; set; } = null!;

    public string FromMailId { get; set; } = null!;

    public string FromName { get; set; } = null!;

    public string? DisplayCompanyName { get; set; }

    public string? DisplayCompanyAddress { get; set; }

    public string? ContactNo { get; set; }

    public string? ContactEmail { get; set; }

    public string? SiteUrl { get; set; }

    public bool IsDefault { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual CompanyMaster? Company { get; set; }
}
