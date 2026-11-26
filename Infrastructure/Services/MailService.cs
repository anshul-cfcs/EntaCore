using Application.DTOs.Communication;
using Application.DTOs.Mail;
using Application.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly CoreDbContext _context;
        private readonly ICommunicationLogService _communicationLogService;
        public MailService(CoreDbContext context, ICommunicationLogService communicationLogService)
        {
            _context = context;
            _communicationLogService = communicationLogService;
        }
        public async Task SendMailHtmlAsync(MailConfigModel mailConfigModel)
        {
            if (mailConfigModel == null) throw new AppException("Mail request cannot be null.");

            if (string.IsNullOrWhiteSpace(mailConfigModel.Subject)) throw new AppException("Mail subject is required.");

            if (string.IsNullOrWhiteSpace(mailConfigModel.Body)) throw new AppException("Mail body is required.");

            var recipientSummary = BuildRecipientSummary(mailConfigModel);
            var communicationLogId = await TryCreateCommunicationLogAsync(mailConfigModel, recipientSummary);

            try
            {
                var smtpConfig = await GetSmtpConfigAsync(mailConfigModel.CompanyId);

                using var mail = new MailMessage
                {
                    Subject = mailConfigModel.Subject,
                    Body = mailConfigModel.Body,
                    From = new MailAddress(smtpConfig.From!, smtpConfig.FromName),
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8
                };

                AddRecipients(mail.To, mailConfigModel.To);

                if (!string.IsNullOrWhiteSpace(mailConfigModel.CC)) AddRecipients(mail.CC, mailConfigModel.CC);

                if (!string.IsNullOrWhiteSpace(mailConfigModel.Bcc)) AddRecipients(mail.Bcc, mailConfigModel.Bcc);

                if (!string.IsNullOrWhiteSpace(mailConfigModel.ReplyTo))
                {
                    foreach (var item in SplitEmails(mailConfigModel.ReplyTo)) mail.ReplyToList.Add(new MailAddress(item));
                }

                if (mailConfigModel.MailCollection != null)
                {
                    foreach (string? key in mailConfigModel.MailCollection.AllKeys)
                    {
                        if (string.IsNullOrWhiteSpace(key)) continue;

                        var displayName = mailConfigModel.MailCollection[key];

                        if (mailConfigModel.AddToType == 2) mail.CC.Add(new MailAddress(key, displayName));
                        else if (mailConfigModel.AddToType == 3) mail.Bcc.Add(new MailAddress(key, displayName));
                        else mail.To.Add(new MailAddress(key, displayName));
                    }
                }

                if (mailConfigModel.MailCollectionCC != null)
                {
                    foreach (string? key in mailConfigModel.MailCollectionCC.AllKeys)
                    {
                        if (string.IsNullOrWhiteSpace(key)) continue;

                        var displayName = mailConfigModel.MailCollectionCC[key];
                        mail.CC.Add(new MailAddress(key, displayName));
                    }
                }

                if (mail.To.Count <= 0)
                    throw new AppException("Require at least one valid mail id to trigger the mail.");

                if (!string.IsNullOrWhiteSpace(mailConfigModel.AttachmentPath))
                {
                    foreach (var attachmentPath in mailConfigModel.AttachmentPath.Split(';', StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (File.Exists(attachmentPath))
                            mail.Attachments.Add(new Attachment(attachmentPath));
                    }
                }

                using var smtpClient = new SmtpClient
                {
                    Host = smtpConfig.SMTPServer!,
                    Port = smtpConfig.SMTPPort,
                    EnableSsl = smtpConfig.SslMode,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(smtpConfig.SenderMailID, smtpConfig.SenderPassword)
                };

                await smtpClient.SendMailAsync(mail);
                await TryMarkCommunicationLogSentAsync(communicationLogId);
            }
            catch (Exception ex)
            {
                await TryMarkCommunicationLogFailedAsync(communicationLogId, ex.Message);
                throw;
            }
        }

        private async Task<SmtpConfigModel> GetSmtpConfigAsync(long CompanyId)
        {

            var config = await _context.MailConfigurationMasters.AsNoTracking().Where(x => x.IsActive && !x.IsDeleted && x.CompanyId == CompanyId)
                .Select(x => new SmtpConfigModel
                {
                    From = x.FromMailId,
                    FromName = x.FromName,
                    SenderMailID = x.SenderMailId,
                    SenderPassword = x.SenderPasswordHash,
                    SMTPServer = x.Smtpserver,
                    SMTPPort = x.Smtpport,
                    SslMode = x.SslMode
                }).FirstOrDefaultAsync();

            if (config == null) throw new AppException("SMTP configuration not found.");

            if (string.IsNullOrWhiteSpace(config.SMTPServer)) throw new AppException("SMTP server is missing.");

            if (config.SMTPPort <= 0) throw new AppException("SMTP port is invalid.");

            if (string.IsNullOrWhiteSpace(config.From)) throw new AppException("SMTP from email is missing.");

            if (string.IsNullOrWhiteSpace(config.SenderMailID)) throw new AppException("SMTP sender email is missing.");

            if (string.IsNullOrWhiteSpace(config.SenderPassword)) throw new AppException("SMTP sender password is missing.");

            return config;
        }

        private async Task<Guid?> TryCreateCommunicationLogAsync(MailConfigModel mailConfigModel, MailRecipientSummary recipientSummary)
        {
            if (mailConfigModel.CompanyId <= 0) return null;

            try
            {
                return await _communicationLogService.CreateAsync(new CommunicationLogRequest
                {
                    CompanyId = Convert.ToInt32(mailConfigModel.CompanyId),
                    CommunicationTypeId = CommunicationType.Email,
                    UserId = mailConfigModel.UserId,
                    Recipient = recipientSummary.To,
                    Cc = recipientSummary.Cc,
                    Bcc = recipientSummary.Bcc,
                    Subject = mailConfigModel.Subject,
                    MessageBody = mailConfigModel.Body,
                    ProviderName = "SMTP",
                    CommunicationStatusId = CommunicationStatus.Queued,
                    CreatedBy = mailConfigModel.UserId
                });
            }
            catch
            {
                return null;
            }
        }

        private async Task TryMarkCommunicationLogSentAsync(Guid? communicationLogId)
        {
            if (!communicationLogId.HasValue) return;

            try
            {
                await _communicationLogService.MarkSentAsync(communicationLogId.Value);
            }
            catch
            {
                // Mail delivery should not be treated as failed because log update failed.
            }
        }

        private async Task TryMarkCommunicationLogFailedAsync(Guid? communicationLogId, string errorMessage)
        {
            if (!communicationLogId.HasValue) return;

            try
            {
                await _communicationLogService.MarkFailedAsync(communicationLogId.Value, errorMessage);
            }
            catch
            {
                // Preserve the original mail error for the caller/background worker.
            }
        }

        private static MailRecipientSummary BuildRecipientSummary(MailConfigModel mailConfigModel)
        {
            var to = SplitEmails(mailConfigModel.To).ToList();
            var cc = SplitEmails(mailConfigModel.CC).ToList();
            var bcc = SplitEmails(mailConfigModel.Bcc).ToList();

            AddNameValueRecipients(mailConfigModel.MailCollection, mailConfigModel.AddToType, to, cc, bcc);
            AddNameValueRecipients(mailConfigModel.MailCollectionCC, 2, to, cc, bcc);

            return new MailRecipientSummary
            {
                To = JoinEmails(to),
                Cc = JoinEmails(cc),
                Bcc = JoinEmails(bcc)
            };
        }

        private static void AddNameValueRecipients(NameValueCollection? recipients, int addToType, List<string> to, List<string> cc, List<string> bcc)
        {
            if (recipients == null) return;

            foreach (string? key in recipients.AllKeys)
            {
                if (string.IsNullOrWhiteSpace(key)) continue;

                if (addToType == 2) cc.Add(key.Trim());
                else if (addToType == 3) bcc.Add(key.Trim());
                else to.Add(key.Trim());
            }
        }

        private static string? JoinEmails(IEnumerable<string> emails)
        {
            var values = emails.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            return values.Count == 0 ? null : string.Join("; ", values);
        }

        private static void AddRecipients(MailAddressCollection collection, string? emails)
        {
            foreach (var email in SplitEmails(emails))
            {
                collection.Add(new MailAddress(email));
            }
        }

        private static IEnumerable<string> SplitEmails(string? emails)
        {
            if (string.IsNullOrWhiteSpace(emails)) return Enumerable.Empty<string>();

            return emails.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x));
        }

        private class MailRecipientSummary
        {
            public string? To { get; set; }

            public string? Cc { get; set; }

            public string? Bcc { get; set; }
        }
    }
}
