using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
    public enum CommunicationStatus
    {
        Queued = 1,
        Sent = 2,
        Failed = 3,
        Delivered = 4
    }
}
