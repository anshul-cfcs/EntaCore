namespace Application.DTOs.Account
{
    public class LoginTokenDetail
    {
        public long UserLoginID { get; set; }
        public Guid EmployeeID { get; set; }
        public int CompanyID { get; set; }
        public int? BranchID { get; set; }
        public int? ZoneID { get; set; }
        public int RoleID { get; set; }
        public string RoleCode { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string AuthCode { get; set; } = string.Empty;
    }
}
