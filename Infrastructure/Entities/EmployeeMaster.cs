using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class EmployeeMaster
{
    public Guid EmployeeId { get; set; }

    public string? TitleName { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? FullName { get; set; }

    public int? CompanyId { get; set; }

    public int? BranchId { get; set; }

    public int? DepartmentId { get; set; }

    public int? DesignationId { get; set; }

    public int? EmploymentTypeId { get; set; }

    public string? CareOfTypeText { get; set; }

    public string? CareOfPersonName { get; set; }

    public int? TeamId { get; set; }

    public Guid? ManagerId { get; set; }

    public Guid? SubManagerId { get; set; }

    public Guid? SuperManagerId { get; set; }

    public int? GenderId { get; set; }

    public string? EmailId { get; set; }

    public DateOnly? JoiningDate { get; set; }

    public string? ProfilePhoto { get; set; }

    public string? SignatureImage { get; set; }

    public string? Pfno { get; set; }

    public DateTime? Dob { get; set; }

    public string? PanNo { get; set; }

    public string? AadharNo { get; set; }

    public bool IsExited { get; set; }

    public DateOnly? ExitDate { get; set; }

    public string? Comments { get; set; }

    public DateTime CreatedAt { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual BranchMaster? Branch { get; set; }

    public virtual ICollection<EmployeeMaster> InverseManager { get; set; } = new List<EmployeeMaster>();

    public virtual ICollection<EmployeeMaster> InverseSuperManager { get; set; } = new List<EmployeeMaster>();

    public virtual EmployeeMaster? Manager { get; set; }

    public virtual EmployeeMaster? SuperManager { get; set; }
}
