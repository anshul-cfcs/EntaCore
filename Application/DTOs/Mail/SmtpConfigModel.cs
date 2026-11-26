using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Mail
{
    public class SmtpConfigModel
    {
        public string? From { get; set; }
        public string? FromName { get; set; }
        public string? SenderMailID { get; set; }
        public string? SenderPassword { get; set; }
        public string? SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public bool SslMode { get; set; }
    }
}
