using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class LoginRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserSalt { get; set; } = string.Empty;
        public string? ClientName { get; set; }
        public string? ClientVersion { get; set; }
        public string? Platform { get; set; }
        public string? ClientIP { get; set; }
    }
}
