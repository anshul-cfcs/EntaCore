using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class UserPasswordUpdateLog
{
    public long PassId { get; set; }

    public long UserLoginId { get; set; }

    public string PasswordHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
