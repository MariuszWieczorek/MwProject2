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

        #region konstruktor, DI
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public CategoryController(ICategoryService categoryService, IUserService userService)
        {
            _categoryService = categoryService;
            _userService = userService;
        }

        #endregion

        #region przeglądanie lista kategorii, pojedyncza kategoria ---
        public IActionResult Categories()
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var categories = _categoryService.GetCategories();

            var vm = new CategoriesViewModel()
            {
                Categories = categories,
                CurrentUser = currentUser
            };

            return View(vm);
        }

        public IActionResult Category(int id)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            var selectedCategory = id == 0 ?
                new Category { Id = 0, Name = string.Empty } :
                _categoryService.GetCategory(id);

            var vm = new CategoryViewModel()
            {
                Category = selectedCategory,
                Heading = "",
                CurrentUser = currentUser
            };

            return View(vm);
        }

        #endregion

        #region edycja/dodawanie/usuwanie kategorii ------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Category(Category category)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            if (!ModelState.IsValid)
            {
                var vm = new CategoryViewModel()
                {
                    Category = category,
                    Heading = category.Id == 0 ?
                     "nowa kategoria" :
                     "edycja kategorii",
                    CurrentUser = currentUser
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
                var currentUser = _userService.GetUser(userId);
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
