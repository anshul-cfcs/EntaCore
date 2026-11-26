using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class ZoneMaster
{
    public int ZoneId { get; set; }

    public string ZoneName { get; set; } = null!;

    public int CompanyId { get; set; }

    public Guid? ZoneManagerId { get; set; }

    public string? ZoneManagerName { get; set; }

    public DateTime CreatedAt { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<BranchMaster> BranchMasters { get; set; } = new List<BranchMaster>();

    public virtual ICollection<EmployeeMaster> EmployeeMasters { get; set; } = new List<EmployeeMaster>();
}
