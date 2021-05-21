using EmailSender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MwProject.Core.Models.Domains;
using MwProject.Core.Services;
using MwProject.Core.ViewModels;
using MwProject.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Controllers
{
    [Authorize]
    public class SendEmailController : Controller
    {

        #region konstruktor, DI
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public SendEmailController(ICategoryService categoryService, IUserService userService)
        {
            _categoryService = categoryService;
            _userService = userService;
        }

        #endregion

        public async Task<ActionResult> SendTestEmail()
        {
            var userId = User.GetUserId();
            
            var emailSenderGm = new EmailSender.EmailSender(new EmailParams
            {
                HostSmtp = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                SenderName = "Mariusz Wieczorek",
                SenderEmail = "mariusz.wieczorek.testy@gmail.com",
                SenderEmailPassword = "rmhfvaurzyxnuztn"
            });

            var emailSenderKa = new EmailSender.EmailSender(new EmailParams
            {
                HostSmtp = "smtp.office365.com",
                Port = 587,
                EnableSsl = true,
                SenderName = "Mariusz Wieczorek",
                SenderEmail = "noreply@kabat.pl",
                SenderEmailPassword = "Ka3aT123!"
            });

            var emailSenderMx = new EmailSender.EmailSender(new EmailParams
            {
                HostSmtp = "kabat-pl.mail.protection.outlook.com",
                Port = 25,
                EnableSsl = false,
                SenderName = "Mariusz Wieczorek",
                SenderEmail = "mwproject@kabaty.pl",
                SenderEmailPassword = null
            });

            var email = new Email()
            {
                Subject = "tytuł wiadomości testowej",
                Message = GenerateHtmlEmail(),
                SentDate = DateTime.Now,
                EmailRecipients = new Collection<Address>()
                {
                    new Address() {Email = "mariusz.wieczorek@kabat.pl", Name = "Mariusz Wieczorek"}
                }
            };


            // Listę odbiorców maila konwertuję to listy adresów
            var listOfEmailRecipients = new List<string>();
            foreach (var emailRecipient in email.EmailRecipients)
                listOfEmailRecipients.Add(emailRecipient.Email);

            await emailSenderMx.Send(email.Subject, email.Message, listOfEmailRecipients);

           // _emailRepository.UpdateSentDate(email, userId);

            return Json(new
            {
                Success = true,
                Message = "Mail został wysłany"
            });
        }

        private string GenerateHtmlEmail()
        {
            var html = $"Treść maila w HTML <br /> <br />";
            html += $@"<table border=1 cellpadding=5  cellspacing=1>
                <tr>
                    <td align=center bgcolor=lightgrey>Wiadomość</td>
                    <td align=center bgcolor=lightgrey>data</td>
                    
                </tr>
                ";


                html +=
                $@"<tr>
                    <td align=center bgcolor=white> kolumna 1</td>
                    <td align=center bgcolor=white> kolumna 1</td>
                </tr>
                ";
        
            html += $@"</table> <br /> <br /> <i>Automatyczna wiadomość wysłana z aplikacji </i>";

            return html;
        }


    }
}
