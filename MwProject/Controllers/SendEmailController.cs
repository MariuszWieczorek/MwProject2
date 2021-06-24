using EmailService;
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
        private readonly IEmailSender _emailSender;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public SendEmailController(ICategoryService categoryService, IUserService userService, IEmailSender emailSender)
        {
            _categoryService = categoryService;
            _userService = userService;
            _emailSender = emailSender;
        }

        #endregion

        public async Task<ActionResult> SendTestEmail()
        {
            var userId = User.GetUserId();
            string subject = "tytuł wiadomości testowej";
            string message = GenerateHtmlEmail();
            var listOfEmailRecipients = new List<string>()
                {
                    "mariusz.wieczorek@kabat.pl"
                };

            var listOfAttachments = new List<string>();

            var mailMessage = _emailSender.CreateMailMessage(subject, message, listOfEmailRecipients, listOfAttachments);
            await _emailSender.SendEmailAsync(mailMessage);

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
