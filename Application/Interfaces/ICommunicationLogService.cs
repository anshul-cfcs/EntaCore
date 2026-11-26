using Application.DTOs.Communication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ICommunicationLogService
    {
        Task<Guid> CreateAsync(CommunicationLogRequest request, CancellationToken cancellationToken = default);

        Task MarkSentAsync(Guid communicationLogId, string? providerMessageId = null, CancellationToken cancellationToken = default);

        Task MarkFailedAsync(Guid communicationLogId, string errorMessage, CancellationToken cancellationToken = default);
    }
}
