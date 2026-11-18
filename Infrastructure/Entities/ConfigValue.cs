using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class ConfigValue
{
    public int ConfigValueId { get; set; }

    public string ConfigKey { get; set; } = null!;

    public string ConfigName { get; set; } = null!;

    public string KeyValue { get; set; } = null!;

    public string? KeyType { get; set; }

    public bool IsSystemConfig { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public int? CompanyId { get; set; }
}
