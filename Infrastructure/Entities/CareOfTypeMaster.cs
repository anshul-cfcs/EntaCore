using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class CareOfTypeMaster
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool IsDeleted { get; set; }

    public int? DisplayOrder { get; set; }
}
