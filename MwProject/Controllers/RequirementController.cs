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

        #region kontstruktor, DI
        private readonly IRequirementService _requirementService;
        private readonly IUserService _userService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public RequirementController(IRequirementService requirementService, IUserService userService)
        {
            _requirementService = requirementService;
            _userService = userService;
        }
        #endregion

        #region Wymagania: przeglądanie listy wymagań, pojedyncze wymaganie ---
        public IActionResult Requirements(int type = 1)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var requirements = _requirementService.GetRequirements().Where(x=>x.Type == type);
            var vm = new RequirementsViewModel()
            {
                Requirements = requirements,
                CurrentUser = currentUser,
                Type = type
            };

            return View(vm);
        }

        public IActionResult Requirement(int id, int type)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var selectedRequirement = id == 0 ?
                _requirementService.NewRequirement() :
                _requirementService.GetRequirement(id);

            var vm = new RequirementViewModel()
            {
                Requirement = selectedRequirement,
                Heading = "",
                CurrentUser = currentUser
            };

            if (id == 0)
            {
                vm.Requirement.Type = type;
            }

            return View(vm);
        }

        #endregion

        #region  wymagania: edycja/dodawanie/usuwanie wymagania ------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Requirement(Requirement requirement)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            if (!ModelState.IsValid)
            {
                var vm = new RequirementViewModel()
                {
                    Requirement = requirement,
                    Heading = requirement.Id == 0 ?
                     "nowe wymaganie" :
                     "edycja wymagania",
                    CurrentUser = currentUser
                };

                return View("Requirement", vm);
            }

            if (requirement.Id == 0)
                _requirementService.AddRequirement(requirement);
            else
                _requirementService.UpdateRequirement(requirement);


            return RedirectToAction("Requirements", "Requirement", new { type = requirement.Type });
        }

        [HttpPost]
        public IActionResult DeleteRequirement(int id)
        {
            try
            {
                var userId = User.GetUserId();
                var currentUser = _userService.GetUser(userId);
                _requirementService.DeleteRequirement(id);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult SetIsActiveToFalse(int id)
        {
            try
            {
                var userId = User.GetUserId();
                var currentUser = _userService.GetUser(userId);
                _requirementService.SetIsActiveToFalse(id);
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
