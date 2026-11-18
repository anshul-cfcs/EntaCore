using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class UserLogin
{
    public long UserLoginId { get; set; }

    public Guid EmployeeId { get; set; }

    public string? EmployeeCode { get; set; }

    public int? RoleId { get; set; }

    public string UserName { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string? PasswordSalt { get; set; }

    public bool IsTwoFactorAuth { get; set; }

    public byte[]? Otp { get; set; }

    public Guid? Otpkey { get; set; }

    public DateTime? OtpvalidUpto { get; set; }

    public byte? OtpwrongCount { get; set; }

    public byte? OtprequestCount { get; set; }

    public DateTime? PasswordChangedAt { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public DateTime? AdminResetDate { get; set; }

    public long? AdminResetBy { get; set; }

    public byte[]? CurrentToken { get; set; }

    public bool IsDms { get; set; }

    public bool IsCase { get; set; }

    public bool IsCompliance { get; set; }

    public bool IsTrading { get; set; }

    public bool IsBulkMail { get; set; }

    public bool IsAllowSms { get; set; }

    public bool IsAllowEmail { get; set; }

    public bool IsLocked { get; set; }

    public bool LoginActive { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }

    public virtual EmployeeMaster Employee { get; set; } = null!;

    public virtual ICollection<LoginLogMaster> LoginLogMasters { get; set; } = new List<LoginLogMaster>();

    public virtual RoleMaster? Role { get; set; }
}
