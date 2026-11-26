using Application.DTOs.Mail;

namespace Application.Interfaces
{
    public interface IMailService
    {
        Task SendMailHtmlAsync(MailConfigModel mailConfigModel);
    }
}
