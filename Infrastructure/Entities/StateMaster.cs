using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class StateMaster
{
    public int StateId { get; set; }

    public int CountryId { get; set; }

    public string StateName { get; set; } = null!;

    public string? StateCode { get; set; }

    public string? StateShortCode { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }
}
