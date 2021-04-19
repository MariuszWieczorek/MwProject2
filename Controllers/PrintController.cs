using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MwProject.Core.Models;
using MwProject.Core.Models.Domains;
using MwProject.Core.Services;
using MwProject.Core.ViewModels;
using MwProject.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Controllers
{
    [Authorize]
    public class PrintController : Controller
    {

        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        private readonly IProjectService _projectService;


        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public PrintController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public ActionResult ProjectTemplate(int id)
        {
            var userId = User.GetUserId();
            var project = _projectService.GetProject(id, userId);

            return View(project);
        }

        /*
        [HttpPost]
        public ActionResult InvoiceToPdf(int id)
        {
            var handle = Guid.NewGuid().ToString();
            var userId = User.GetUserId();
            var invoice = _projectService.GetProject(id, userId);

            TempData[handle] = GetPdfContent(invoice);


            return Json(new
            {
                Success = true,
                FileGuid = handle,
                FileName = $@"Faktura_{invoice.Id}.pdf"
            });
        }

        private byte[] GetPdfContent(Invoice invoice)
        {
            var pdfResult = new ViewAsPdf(@"InvoiceTemplate", invoice)
            {
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait
            };

            return pdfResult.BuildFile(ControllerContext);
        }

        public ActionResult DownloadInvoicePdf(string fileGuid, string fileName)
        {

            if (TempData[fileGuid] == null)
                throw new Exception("Błąd przy próbie eksportu faktury do PDF.");

            var data = TempData[fileGuid] as byte[];

            return File(data, "application/pdf", fileName);
        }

        */

    }
}
