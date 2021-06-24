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
    public class RankingCategoryController : Controller
    {

        #region konstruktor, DI
        private readonly IRankingCategoryService _rankingCategoryService;
        private readonly IUserService _userService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public RankingCategoryController(IRankingCategoryService rankingCategoryService, IUserService userService)
        {
            _rankingCategoryService = rankingCategoryService;
            _userService = userService;
        }

        #endregion

        #region przeglądanie lista kategorii, pojedyncza kategoria ---
        public IActionResult RankingCategories()
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var rankingCategories = _rankingCategoryService.GetRankingCategories();

            var vm = new RankingCategoriesViewModel()
            {
                RankingCategories = rankingCategories,
                CurrentUser = currentUser
            };

            return View(vm);
        }

        public IActionResult RankingCategory(int id)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            var selectedRankingCategory = id == 0 ?
                _rankingCategoryService.NewRankingCategory() :
                _rankingCategoryService.GetRankingCategory(id);

            var vm = new RankingCategoryViewModel()
            {
                RankingCategory = selectedRankingCategory,
                Heading = id == 0 ? "nowa kategoria": $"{selectedRankingCategory.Name}",
                CurrentUser = currentUser
            };

            return View(vm);
        }

        #endregion

        #region edycja/dodawanie/usuwanie kategorii ------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RankingCategory(RankingCategory rankingCategory)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            if (!ModelState.IsValid)
            {
                var vm = new RankingCategoryViewModel()
                {
                    RankingCategory = rankingCategory,
                    Heading = rankingCategory.Id == 0 ?
                     "nowa kategoria" :
                     "edycja kategorii",
                    CurrentUser = currentUser
                };

                return View("RankingCategory", vm);
            }

            if (rankingCategory.Id == 0)
                _rankingCategoryService.AddRankingCategory(rankingCategory);
            else
                _rankingCategoryService.UpdateRankingCategory(rankingCategory);


            return RedirectToAction("RankingCategories", "RankingCategory");
        }

        [HttpPost]
        public IActionResult DeleteRankingCategory(int id)
        {
            try
            {
                var userId = User.GetUserId();
                var currentUser = _userService.GetUser(userId);
                _rankingCategoryService.DeleteRankingCategory(id);
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
