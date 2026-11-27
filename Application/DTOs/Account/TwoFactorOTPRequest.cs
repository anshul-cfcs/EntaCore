using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class TwoFactorOTPRequest
    {
        public Guid OTPKey { get; set; }
        public string OTP { get; set; } = string.Empty;
        public string UserSalt { get; set; } = string.Empty;
        public string? ClientName { get; set; }
        public string? ClientVersion { get; set; }
        public string? Platform { get; set; }
        public string? ClientIP { get; set; }
    }
}
