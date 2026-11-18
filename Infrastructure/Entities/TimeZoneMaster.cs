using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class TimeZoneMaster
{
    public int TimeZoneId { get; set; }

    public string TimeZoneName { get; set; } = null!;

    public string? DisplayName { get; set; }

    public string UtcOffset { get; set; } = null!;

    public bool IsCurrentlyDst { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }
}
