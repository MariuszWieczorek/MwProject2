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
    public class RequirementController : Controller
    {
        
        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        private readonly IRequirementService _requirementService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public RequirementController(IRequirementService requirementService)
        {
            _requirementService = requirementService;
        }

        #region Wymagania: przeglądanie listy wymagań, pojedyncze wymaganie ---
        public IActionResult Requirements()
        {
            var userId = User.GetUserId();
            var requirement = _requirementService.GetRequirements();

            return View(requirement);
        }

        public IActionResult Requirement(int id)
        {
            var userId = User.GetUserId();
            var selectedRequirement = id == 0 ?
                _requirementService.NewRequirement() :
                _requirementService.GetRequirement(id);

            var vm = new RequirementViewModel()
            {
                Requirement = selectedRequirement,
                Heading = ""
            };

            return View(vm);
        }

        #endregion

        #region  wymagania: edycja/dodawanie/usuwanie wymagania ------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Requirement(Requirement requirement)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = new RequirementViewModel()
                {
                    Requirement = requirement,
                    Heading = requirement.Id == 0 ?
                     "nowe wymaganie" :
                     "edycja wymagania"
                };

                return View("Requirement", vm);
            }

            if (requirement.Id == 0)
                _requirementService.AddRequirement(requirement);
            else
                _requirementService.UpdateRequirement(requirement);


            return RedirectToAction("Requirements", "Requirement");
        }

        [HttpPost]
        public IActionResult DeleteRequirement(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _requirementService.DeleteRequirement(id);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message });
            }

            return Json(new { success = true });
        }

        #endregion

    }
}
