using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class TitleMaster
{
    public int RecordId { get; set; }

    public string? Name { get; set; }

    public bool IsDeleted { get; set; }
}
