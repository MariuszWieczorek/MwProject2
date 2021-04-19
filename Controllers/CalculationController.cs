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
    public class CalculationController : Controller
    {
        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        private readonly ICalculationService _calculationService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public CalculationController(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        // wyświetlamy wybraną kalkulację lub pusty objekt
        public IActionResult Calculation(int projectId, int id)
        {
            var userId = User.GetUserId();
            var selectedCalculation = id == 0 ?
                _calculationService.NewCalculation(projectId,userId) :
                _calculationService.GetCalculation(projectId,id,userId);

            var vm = new CalculationViewModel()
            {
                Calculation = selectedCalculation,
                Heading = id == 0 ? $"nowa {projectId}" : $"edycja {projectId}"
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Calculation(CalculationViewModel selectedCalculation)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = new CalculationViewModel()
                {
                    Calculation = selectedCalculation.Calculation,
                    Heading = selectedCalculation.Calculation.Id == 0 ? "nowa" : "edycja"
                };

                return View("Calculation", vm);
            }

            if (selectedCalculation.Calculation.Id == 0)
                _calculationService.AddCalculation(selectedCalculation.Calculation, userId);
            else
                _calculationService.UpdateCalculation(selectedCalculation.Calculation, userId);


            return RedirectToAction("Project","Project", new { id = selectedCalculation.Calculation.ProjectId });

        }


        [HttpPost]
        public IActionResult DeleteCalculation(int projectId, int id)
        {
            try
            {
                var userId = User.GetUserId();
                _calculationService.DeleteCalculation(projectId, id, userId);
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
