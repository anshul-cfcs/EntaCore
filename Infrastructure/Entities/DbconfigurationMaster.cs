using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class DbconfigurationMaster
{
    public Guid DbconfigurationId { get; set; }

    public int CompanyId { get; set; }

    public string? ServerName { get; set; }

    public string? Dbname { get; set; }

    public string? UserId { get; set; }

    public string? PwdHash { get; set; }

    public bool? ServerCertificate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }
}
