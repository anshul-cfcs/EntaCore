using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class ChangePasswordRequest
    {
        public string PasswordHash { get; set; } = string.Empty;

    }
}
