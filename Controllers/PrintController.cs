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

        #region konstruktor, mechanizm DI 

        private readonly IProjectService _projectService;
        private readonly ICategoryService _categoryService;
        private readonly IProductGroupService _productGroupService;
        private readonly IUserService _userService;

        private readonly int _itemPerPage = 30;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public PrintController(IProjectService projectService,
                                 ICategoryService categoryService,
                                 IProductGroupService productGroupService,
                                 IUserService userService
                                )
        {
            _projectService = projectService;
            _categoryService = categoryService;
            _productGroupService = productGroupService;
            _userService = userService;
        }

        #endregion

        #region Pojedynczy projekt - wydruk
        public IActionResult ProjectRaport(int id)
        {
            var userId = User.GetUserId();

            var currentUser = _userService.GetUser(userId);

            var selectedProject = id == 0 ?
                _projectService.NewProject(userId) :
                _projectService.GetProject(id, userId);

            ApplicationUser acceptedBy = new();
            ApplicationUser confirmedBy = new();
            ApplicationUser calculationConfirmedBy = new();
            ApplicationUser estiamtedSalesConfirmedBy = new();
            ApplicationUser qualityRequirementsConfirmedBy = new();
            ApplicationUser economicRequirementsConfirmedBy = new();
            ApplicationUser technicalPropertiesConfirmedBy = new();

            if (selectedProject.AcceptedBy != null)
            {
                acceptedBy = _userService.GetUser(selectedProject.AcceptedBy);
            }

            if (selectedProject.ConfirmedBy != null)
            {
                confirmedBy = _userService.GetUser(selectedProject.ConfirmedBy);
            }

            if (selectedProject.CalculationConfirmedBy != null)
            {
                calculationConfirmedBy = _userService.GetUser(selectedProject.CalculationConfirmedBy);
            }

            if (selectedProject.EstimatedSalesConfirmedBy != null)
            {
                estiamtedSalesConfirmedBy = _userService.GetUser(selectedProject.EstimatedSalesConfirmedBy);
            }

            if (selectedProject.QualityRequirementsConfirmedBy != null)
            {
                qualityRequirementsConfirmedBy = _userService.GetUser(selectedProject.QualityRequirementsConfirmedBy);
            }

            if (selectedProject.EconomicRequirementsConfirmedBy != null)
            {
                economicRequirementsConfirmedBy = _userService.GetUser(selectedProject.EconomicRequirementsConfirmedBy);
            }

            if (selectedProject.TechnicalProportiesConfirmedBy != null)
            {
                technicalPropertiesConfirmedBy = _userService.GetUser(selectedProject.TechnicalProportiesConfirmedBy);
            }

            var vm = new ProjectViewModel()
            {
                Project = selectedProject,
                ProductGroups = _productGroupService.GetProductGroups(),
                Categories = _categoryService.GetCategories(),
                Heading = selectedProject.Id == 0 ?
                      "Nowy Projekt" :
                     $"Edycja Projektu: {selectedProject.Number}",
                AcceptedBy = acceptedBy,
                ConfirmedBy = confirmedBy,
                CalculationConfirmedBy = calculationConfirmedBy,
                EstimatedSalesConfirmedBy = estiamtedSalesConfirmedBy,
                QualityRequirementsConfirmedBy = qualityRequirementsConfirmedBy,
                EconomicRequirementsConfirmedBy = economicRequirementsConfirmedBy,
                TechnicalPropertiesConfirmedBy = technicalPropertiesConfirmedBy,
                CurrentUser = currentUser
            };

            return View(vm);
        }

        #endregion

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
