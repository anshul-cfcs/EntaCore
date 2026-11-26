using System.Text;

namespace Infrastructure.Services.MailTemplate
{
    public class AccountMailTemplates
    {
        public static string BuildLoginOtpBody(string fullName, string otp)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div>Dear " + fullName + "</div>");
            sb.AppendLine("<div><br />Your OTP for login is - <strong>" + otp + "</strong></div>");

            return sb.ToString();
        }

        public static string BuildForgotPasswordOtpBody(string fullName, string otp, string resetLink)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div style='font-family:verdana; font-size:12px;'>Dear " + fullName + "</div>");
            sb.AppendLine("<br />We received a password reset request.");
            sb.AppendLine("<br /><br />If this was done by you, please use the below OTP.");

            if (!string.IsNullOrWhiteSpace(resetLink))
            {
                sb.AppendLine("<br /><br />Reset Link:");
                sb.AppendLine("<br /><a href='" + resetLink + "'>" + resetLink + "</a>");
            }

            sb.AppendLine("<br /><br />OTP : <strong>" + otp + "</strong>");

            return sb.ToString();
        }

        public static string BuildPasswordChangedBody(string fullName)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<div style='font-family:verdana; font-size:12px;'>Dear {fullName}</div>");
            sb.AppendLine("<br /><br />Your Theo CRM password was updated successfully.");
            sb.AppendLine("</div>");

            return sb.ToString();
        }

        public static string BuildCompanyAdminPasswordBody(string fullName, string userName, string password)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div style='font-family:verdana; font-size:12px;'>Dear " + fullName + "</div>");
            sb.AppendLine("<br /><br />Your company admin account has been created.");
            sb.AppendLine("<br /><br /><strong>Username:</strong> " + userName);
            sb.AppendLine("<br /><strong>Password:</strong> " + password);
            sb.AppendLine("<br /><br />Please change your password after first login.");

            return sb.ToString();
        }
    }
}
