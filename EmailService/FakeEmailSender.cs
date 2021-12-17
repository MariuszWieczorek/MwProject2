using EmailService.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmailService
{
    public class FakeEmailSender : IEmailSender
    {
        private SmtpClient _smtp;
        private MailMessage _mail;
        private readonly EmailConfiguration _emailConfiguration;
     
        public FakeEmailSender(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

     
        public MailMessage CreateMailMessage(string subject, string body, List<string> emailRecipients, List<string> attachments)
        {
            _mail = new MailMessage();
            _mail.From = new MailAddress(_emailConfiguration.SenderEmail, _emailConfiguration.SenderName);
            _mail.Subject = subject;
            _mail.IsBodyHtml = true;
            _mail.BodyEncoding = Encoding.UTF8;
            _mail.SubjectEncoding = Encoding.UTF8;

            foreach (var address in emailRecipients)
                _mail.To.Add(new MailAddress(address));

            foreach (var attachment in attachments)
            {
                var data = new Attachment(attachment, MediaTypeNames.Application.Octet);
                _mail.Attachments.Add(data);
            }
            // _mail.Body = body; też by zadziałało
            // aby mail trafił do skrzynki odbiorczej, a nie do spamu jest mnóstwo zasad
            // tworzymy maila w dwóch wersjach: bez tagów html oraz z tagami <html>, <head>, <body>

            _mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(
                body.StripHTML(),
                null,
                MediaTypeNames.Text.Plain));

            _mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(
                 AddHtmlHeader(body),
                 null,
                 MediaTypeNames.Text.Html));

            return _mail;
        }


        private string AddHtmlHeader(string text)
        {
            return $@"
            <html>    
                <head>
                </head>
                <body>
            <div style='font-size: 16px; padding: 10px; font-family: Arial; line-height: 1.4'>
                {text}
            </div>
            </body>
            </html>
            ";
        }

        public async Task SendEmailAsync(MailMessage mail)
        {
            _smtp = new SmtpClient()
            {
                Host = _emailConfiguration.SmtpServer,
                EnableSsl = _emailConfiguration.EnableSsl,
                Port = _emailConfiguration.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailConfiguration.SenderEmail, _emailConfiguration.SenderEmailPassword)
            };

            _smtp.SendCompleted += OnSendCompleted;

            // await _smtp.SendMailAsync(_mail);
        }

        public void SendEmail(MailMessage mail)
        {
            _smtp = new SmtpClient()
            {
                Host = _emailConfiguration.SmtpServer,
                EnableSsl = _emailConfiguration.EnableSsl,
                Port = _emailConfiguration.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailConfiguration.SenderEmail, _emailConfiguration.SenderEmailPassword)
            };

            _smtp.SendCompleted += OnSendCompleted;

           // _smtp.Send(_mail);
        }

        private void OnSendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            _smtp.Dispose();
            _mail.Dispose();
        }
    }
}
