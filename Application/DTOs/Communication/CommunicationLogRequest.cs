using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Communication
{
    public class CommunicationLogRequest
    {
        public int CompanyId { get; set; }

        public CommunicationType CommunicationTypeId { get; set; }

        public long? UserId { get; set; }

        public string? Recipient { get; set; }

        public string? Cc { get; set; }

        public string? Bcc { get; set; }

        public string? Subject { get; set; }

        public string? MessageBody { get; set; }

        public string? ProviderName { get; set; }

        public string? ProviderMessageId { get; set; }

        public CommunicationStatus CommunicationStatusId { get; set; } = CommunicationStatus.Queued;

        public string? ErrorMessage { get; set; }

        public long? CreatedBy { get; set; }
    }
}
