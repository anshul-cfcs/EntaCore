using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.SuperAdmin
{
    public class CompanyFilters
    {
        [Required(ErrorMessage = "Page Index is Required")]
        public int PageNo { get; set; } = 1;

        [Required(ErrorMessage = "Page Size is Required")]
        public int PageSize { get; set; } = int.MaxValue;

        public string SortColumnName { get; set; } = "";
        public bool SortDirection { get; set; } = false;

        public string SearchString { get; set; } = "";

        // 2 = All, 1 = Active, 0 = Inactive
        public int Active { get; set; } = 2;
    }
}
