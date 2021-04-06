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
    public class CategoryController : Controller
    {
        
        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        private readonly ICategoryService _categoryService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        #region Kategorie: przeglądanie lista kategorii, pojedyncza kategoria ---
        public IActionResult Categories()
        {
            var userId = User.GetUserId();
            var categories = _categoryService.GetCategories();

            return View(categories);
        }

        public IActionResult Category(int id)
        {
            var userId = User.GetUserId();
            var selectedCategory = id == 0 ?
                new Category { Id = 0, Name = string.Empty } :
                _categoryService.GetCategory(id);

            var vm = new CategoryViewModel()
            {
                Category = selectedCategory,
                Heading = ""
            };

            return View(vm);
        }

        #endregion

        #region Kategorie: edycja/dodawanie/usuwanie kategorii ------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Category(Category category)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = new CategoryViewModel()
                {
                    Category = category,
                    Heading = category.Id == 0 ?
                     "nowa kategoria" :
                     "edycja kategorii"
                };

                return View("Category", vm);
            }

            if (category.Id == 0)
                _categoryService.AddCategory(category);
            else
                _categoryService.UpdateCategory(category);


            return RedirectToAction("Categories", "Category");
        }

        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _categoryService.DeleteCategory(id);
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
