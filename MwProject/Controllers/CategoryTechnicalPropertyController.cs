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
    public class CategoryTechnicalPropertyController : Controller
    {
        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        private readonly ICategoryTechnicalPropertyService _categoryTechnicalPropertyService;
        private readonly ITechnicalPropertyService _technicalPropertyService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public CategoryTechnicalPropertyController(ICategoryTechnicalPropertyService categoryTechnicalPropertyService,
                                            ITechnicalPropertyService technicalPropertyService)
        {
            _categoryTechnicalPropertyService = categoryTechnicalPropertyService;
            _technicalPropertyService = technicalPropertyService;
        }

        // wyświetlamy wybraną kalkulację lub pusty objekt
        public IActionResult CategoryTechnicalProperty(int categoryId, int id)
        {
            var userId = User.GetUserId();
            var selectedCategoryTechnicalProperty = id == 0 ?
                _categoryTechnicalPropertyService.NewCategoryTechnicalProperty(categoryId,userId) :
                _categoryTechnicalPropertyService.GetCategoryTechnicalProperty(categoryId,id,userId);
            
            var vm = new CategoryTechnicalPropertyViewModel()
            {
                CategoryTechnicalProperty = selectedCategoryTechnicalProperty,
                Heading = id == 0 ? $"nowy parametr" : $"edycja parametru",
                TechnicalProperties = _technicalPropertyService.GetTechnicalProperties()
            };
            
             return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoryTechnicalProperty(CategoryTechnicalPropertyViewModel selectedCategoryTechnicalProperty)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = new CategoryTechnicalPropertyViewModel()
                {
                    CategoryTechnicalProperty = selectedCategoryTechnicalProperty.CategoryTechnicalProperty,
                    Heading = selectedCategoryTechnicalProperty.CategoryTechnicalProperty.Id == 0 ? "nowy parametr" : "edycja parametru",
                    TechnicalProperties = _technicalPropertyService.GetTechnicalProperties()
                };

                return View("CategoryTechnicalProperty", vm);
            }

            if (selectedCategoryTechnicalProperty.CategoryTechnicalProperty.Id == 0)
                _categoryTechnicalPropertyService.AddCategoryTechnicalProperty(selectedCategoryTechnicalProperty.CategoryTechnicalProperty, userId);
            else
                _categoryTechnicalPropertyService.UpdateCategoryTechnicalProperty(selectedCategoryTechnicalProperty.CategoryTechnicalProperty, userId);


            return RedirectToAction("Category", "Category",
                new { id = selectedCategoryTechnicalProperty.CategoryTechnicalProperty.CategoryId });

        }


        [HttpPost]
        public IActionResult DeleteCategoryTechnicalProperty(int categoryId, int id)
        {
            try
            {
                var userId = User.GetUserId();
                _categoryTechnicalPropertyService.DeleteCategoryTechnicalProperty(categoryId, id, userId);
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
