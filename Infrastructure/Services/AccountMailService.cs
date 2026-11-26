using Application.DTOs.Account;
using Application.DTOs.Mail;
using Application.Interfaces;
using Domain.CommonModels;
using Infrastructure.Data;
using Infrastructure.Services.MailTemplate;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Text;

namespace Infrastructure.Services
{

    public class AccountMailService : IAccountMailService
    {
        private readonly IMailService _mailService;
        private readonly CoreDbContext _context;
        public AccountMailService(CoreDbContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }
        private string GetHeader()
        {
            return "<html><head><style type=\"text/css\">body{font-size: 11px;font-family: Verdana, sans-serif; line-height: 18px;} td{ font-size:11px }</style></head><body>";
        }
        private string GetFooter(string companyName, string companyAddress, string contactNo, string contactEmail)
        {
            var sbFooter = new StringBuilder();

            sbFooter.Append("<br />");
            sbFooter.Append("--------");
            sbFooter.Append("<div style='font-family:Verdana;font-size:10pt'>With best regards <br />");
            sbFooter.Append("<strong>" + companyName + "</strong></div><br />");
            sbFooter.Append("<div style='font-family:Verdana;font-size:10pt'>");
            sbFooter.Append("<strong>" + companyAddress + "</strong><br/>");
            sbFooter.Append("Contact No. : " + contactNo + "<br/>");
            sbFooter.Append("Mail : " + contactEmail + "<br/>");
            sbFooter.Append("<br/><hr><div style='text-align:center;'>This is a system generated mail</div>");
            sbFooter.Append("</div></body></html>");

            return sbFooter.ToString();
        }
        private async Task<MailFooterInfo> GetMailFooterInfoAsync(int CompanyId)
        {
            var data = await _context.MailConfigurationMasters.AsNoTracking().Where(x => x.CompanyId == CompanyId && x.IsActive && !x.IsDeleted)
                .Select(x => new MailFooterInfo
                {
                    CompanyName = x.Company.CompanyName ?? string.Empty,
                    CompanyAddress = x.Company.AddressLine1 ?? string.Empty, // I have to solve the following thing
                    ContactNo = (!string.IsNullOrEmpty(x.Company.Mobile) ? x.Company.Mobile : x.Company.Phone) ?? string.Empty,
                    ContactEmail = x.ContactEmail ?? string.Empty,
                    SiteUrl = x.SiteUrl ?? string.Empty
                }).FirstOrDefaultAsync();

            return data ?? new MailFooterInfo();
        }
        public async Task SendLoginOtpAsync(LoginResultDto result)
        {
            if (result == null || string.IsNullOrWhiteSpace(result.EmailID))
                return;

            var footer = await GetMailFooterInfoAsync(result.CompanyID);
            var subject = "Login two factor authentication OTP : " + footer.CompanyName;
            var body = GetHeader() + AccountMailTemplates.BuildLoginOtpBody(result.FullName, result.OTP ?? string.Empty) + GetFooter(footer.CompanyName, footer.CompanyAddress, footer.ContactNo, footer.ContactEmail);

            var mail = new MailConfigModel
            {
                Subject = subject,
                Body = body,
                AddToType = 1,
                MailCollection = new NameValueCollection
            {
                { result.EmailID, result.FullName }
            },
                CompanyId = result.CompanyID,
            };

            await _mailService.SendMailHtmlAsync(mail);
        }

        private class MailFooterInfo
        {
            public string CompanyName { get; set; } = string.Empty;
            public string CompanyAddress { get; set; } = string.Empty;
            public string ContactNo { get; set; } = string.Empty;
            public string ContactEmail { get; set; } = string.Empty;
            public string SiteUrl { get; set; } = string.Empty;
        }
    }
}
