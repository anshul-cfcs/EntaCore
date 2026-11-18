using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class LoginLogMaster
{
    public Guid LoginLogId { get; set; }

    public Guid? EmployeeId { get; set; }

    public long? UserLoginId { get; set; }

    public int? CompanyId { get; set; }

    public string? UserName { get; set; }

    public int? LoginTypeId { get; set; }

    public string? ClientName { get; set; }

    public string? ClientVersion { get; set; }

    public string? Platform { get; set; }

    public string? ClientIp { get; set; }

    public string? UserAgent { get; set; }

    public byte[]? SessionId { get; set; }

    public DateTime? LoginTime { get; set; }

    public DateTime? LogOutTime { get; set; }

    public bool IsLoginSuccess { get; set; }

    public string? FailureReason { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual CompanyMaster? Company { get; set; }

    public virtual EmployeeMaster? Employee { get; set; }

    public virtual UserLogin? UserLogin { get; set; }
}
