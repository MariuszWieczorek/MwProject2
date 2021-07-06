using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class EstimatedSalesValueController : Controller
    {
        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        private readonly IEstimatedSalesValueService _estimatedSalesValueService;
        private readonly IProjectService _projectService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public EstimatedSalesValueController(IEstimatedSalesValueService estimatedSalesValue,
            IProjectService projectService)
        {
            _estimatedSalesValueService = estimatedSalesValue;
            _projectService = projectService;
        }

        // wyświetlamy wybraną kalkulację lub pusty objekt
        public IActionResult EstimatedSalesValue(int projectId, int id)
        {
            var userId = User.GetUserId();
            var selectedEstimatedSalesValue = id == 0 ?
                _estimatedSalesValueService.NewEstimatedSalesValue(projectId,userId) :
                _estimatedSalesValueService.GetEstimatedSalesValue(projectId,id,userId);

            var vm = new EstimatedSalesValueViewModel()
            {
                EstimatedSalesValue = selectedEstimatedSalesValue,
                Heading = id == 0 ? $"nowa przewidywana sprzedaż" : $"edycja przewidywanej sprzedaży"
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EstimatedSalesValue(EstimatedSalesValueViewModel estimatedSalesValueViewModel)
        {
            var userId = User.GetUserId();
            var estimatedSalesValue = estimatedSalesValueViewModel.EstimatedSalesValue;

            if (!ModelState.IsValid)
            {
                var vm = new EstimatedSalesValueViewModel()
                {
                    EstimatedSalesValue = estimatedSalesValue,
                    Heading = estimatedSalesValue.Id == 0 ? $"nowa przewidywana sprzedaż" : $"edycja przewidywanej sprzedaży"
                };

                return View("EstimatedSalesValue", vm);
            }

            if (estimatedSalesValueViewModel.EstimatedSalesValue.Id == 0)
                _estimatedSalesValueService.AddEstimatedSalesValue(estimatedSalesValue, userId);
            else
                _estimatedSalesValueService.UpdateEstimatedSalesValue(estimatedSalesValue, userId);

            _projectService.CalculatePriorityOfProject(estimatedSalesValue.ProjectId,userId);

            return RedirectToAction("Project","Project", new { id = estimatedSalesValue.ProjectId, tab = "sales" });

        }

        [HttpPost]
        public IActionResult DeleteEstimatedSalesValue(int projectId, int id)
        {
            try
            {
                var userId = User.GetUserId();
                _estimatedSalesValueService.DeleteEstimatedSalesValue(projectId,id,userId);
                _projectService.CalculatePriorityOfProject(projectId, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message });
            }

            return Json(new { success = true });
        }

    }
}
