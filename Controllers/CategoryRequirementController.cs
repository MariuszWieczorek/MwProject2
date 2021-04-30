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
    public class CategoryRequirementController : Controller
    {
        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        private readonly ICategoryRequirementService _categoryRequirementService;
        private readonly IRequirementService _requirementService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public CategoryRequirementController(ICategoryRequirementService categoryRequirementService,
                                            IRequirementService requirementService)
        {
            _categoryRequirementService = categoryRequirementService;
            _requirementService = requirementService;
        }

        // wyświetlamy wybraną kalkulację lub pusty objekt
        public IActionResult CategoryRequirement(int categoryId, int id, int type)
        {
            var userId = User.GetUserId();
            var selectedCategoryRequirement = id == 0 ?
                _categoryRequirementService.NewCategoryRequirement(categoryId,userId, type) :
                _categoryRequirementService.GetCategoryRequirement(categoryId,id,userId);
            
            var vm = new CategoryRequirementViewModel()
            {
                CategoryRequirement = selectedCategoryRequirement,
                Heading = id == 0 ? $"nowy parametr" : $"edycja parametru",
                Requirements = _requirementService.GetRequirements()
            };

            if (type != 0)
            {
                vm.Requirements = _requirementService.GetRequirements().Where(x => x.Type == type);
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoryRequirement(CategoryRequirementViewModel selectedCategoryRequirement)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = new CategoryRequirementViewModel()
                {
                    CategoryRequirement = selectedCategoryRequirement.CategoryRequirement,
                    Heading = selectedCategoryRequirement.CategoryRequirement.Id == 0 ? "nowy parametr" : "edycja parametru",
                    Requirements = _requirementService.GetRequirements()
                };

                return View("CategoryRequirement", vm);
            }

            if (selectedCategoryRequirement.CategoryRequirement.Id == 0)
                _categoryRequirementService.AddCategoryRequirement(selectedCategoryRequirement.CategoryRequirement, userId);
            else
                _categoryRequirementService.UpdateCategoryRequirement(selectedCategoryRequirement.CategoryRequirement, userId);


            return RedirectToAction("Category", "Category",
                new { id = selectedCategoryRequirement.CategoryRequirement.CategoryId });

        }


        [HttpPost]
        public IActionResult DeleteCategoryRequirement(int categoryId, int id)
        {
            try
            {
                var userId = User.GetUserId();
                _categoryRequirementService.DeleteCategoryRequirement(categoryId, id, userId);
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
