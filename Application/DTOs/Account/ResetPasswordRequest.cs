using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class ResetPasswordRequest
    {
        public Guid ResetSalt { get; set; }
        public string OTP { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
