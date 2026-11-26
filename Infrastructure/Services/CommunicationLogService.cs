using Application.DTOs.Communication;
using Application.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class CommunicationLogService : ICommunicationLogService
    {
        private readonly CoreDbContext _context;

        public CommunicationLogService(CoreDbContext context)
        {
            _context = context;
        }
        public Task<Guid> CreateAsync(CommunicationLogRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task MarkFailedAsync(Guid communicationLogId, string errorMessage, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task MarkSentAsync(Guid communicationLogId, string? providerMessageId = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
