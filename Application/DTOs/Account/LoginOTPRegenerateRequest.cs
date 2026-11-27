using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class LoginOTPRegenerateRequest
    {
        public Guid OTPKey { get; set; }
    }
}
