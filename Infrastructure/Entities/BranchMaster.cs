using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class BranchMaster
{
    public int BranchId { get; set; }

    public string BranchName { get; set; } = null!;

    public int CompanyId { get; set; }

    public Guid? BranchManagerId { get; set; }

    public string? BranchManagerName { get; set; }

    public DateTime CreatedAt { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<EmployeeMaster> EmployeeMasters { get; set; } = new List<EmployeeMaster>();
}
