using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.CommonModels
{
    public class CompanyFolderNames
    {
        public const string Logo = "Logo";
        public const string Employee = "Employee";
        public const string Documents = "Documents";
        public const string Export = "Export";
        public const string Customer = "Customer";
        public const string Case = "Case";
        public const string DMS = "DMS";
        public const string Compliance = "Compliance";
        public const string Trading = "Trading";
        public const string BulkMail = "BulkMail";

        public static readonly string[] All =
        {
            Logo,
            Employee,
            Documents,
            Export,
            Customer,
            Case,
            DMS,
            Compliance,
            Trading,
            BulkMail
        };
    }
}
