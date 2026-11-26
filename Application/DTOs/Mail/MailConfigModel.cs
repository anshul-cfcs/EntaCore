using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Application.DTOs.Mail
{
    public class MailConfigModel
    {
        public long CompanyId { get; set; }
        public long? UserId { get; set; }
        public string? To { get; set; }
        public string? CC { get; set; }
        public string? Bcc { get; set; }
        public string? ReplyTo { get; set; }

        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        public int AddToType { get; set; } = 1;

        public NameValueCollection? MailCollection { get; set; }
        public NameValueCollection? MailCollectionCC { get; set; }

        public string? AttachmentPath { get; set; }
    }
}
