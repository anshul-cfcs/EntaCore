using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.CommonModels
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public bool NextPageAvailable { get; set; }
        public List<T> Items { get; set; } = new();
    }
}
