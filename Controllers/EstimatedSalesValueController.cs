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

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public EstimatedSalesValueController(IEstimatedSalesValueService estimatedSalesValue)
        {
            _estimatedSalesValueService = estimatedSalesValue;
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
                Heading = id == 0 ? $"nowa {projectId}" : $"edycja {projectId}"
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EstimatedSalesValue(EstimatedSalesValueViewModel estimatedSalesValueViewModel)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = new EstimatedSalesValueViewModel()
                {
                    EstimatedSalesValue = estimatedSalesValueViewModel.EstimatedSalesValue,
                    Heading = estimatedSalesValueViewModel.EstimatedSalesValue.Id == 0 ? $"nowa {estimatedSalesValueViewModel.EstimatedSalesValue.ProjectId}" : $"edycja {estimatedSalesValueViewModel.EstimatedSalesValue.ProjectId}"
                };

                return View("EstimatedSalesValue", vm);
            }

            if (estimatedSalesValueViewModel.EstimatedSalesValue.Id == 0)
                _estimatedSalesValueService.AddEstimatedSalesValue(estimatedSalesValueViewModel.EstimatedSalesValue, userId);
            else
                _estimatedSalesValueService.UpdateEstimatedSalesValue(estimatedSalesValueViewModel.EstimatedSalesValue, userId);


            return RedirectToAction("Project","Project", new { id = estimatedSalesValueViewModel.EstimatedSalesValue.ProjectId });

        }

        [HttpPost]
        public IActionResult DeleteEstimatedSalesValue(int projectId, int id)
        {
            try
            {
                var userId = User.GetUserId();
                _estimatedSalesValueService.DeleteEstimatedSalesValue(projectId,id,userId);
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
