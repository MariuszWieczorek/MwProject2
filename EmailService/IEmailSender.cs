using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(MailMessage message);
        void SendEmail(MailMessage message);
        MailMessage CreateMailMessage(string subject, string body, List<string> emailRecipients, List<string> attachments);
    }
}
