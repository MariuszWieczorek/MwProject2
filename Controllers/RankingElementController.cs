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
    public class RankingElementController : Controller
    {

        #region konstruktor, DI
        private readonly IRankingElementService _rankingElementService;
        private readonly IUserService _userService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public RankingElementController(IRankingElementService rankingElementService, IUserService userService)
        {
            _rankingElementService = rankingElementService;
            _userService = userService;
        }

        #endregion

        #region przeglądanie lista kategorii, pojedyncza kategoria ---
       public IActionResult RankingElement(int rankingCategoryId, int id)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            var selectedRankingElement = id == 0 ?
                _rankingElementService.NewRankingElement(rankingCategoryId,userId) :
                _rankingElementService.GetRankingElement(rankingCategoryId,id, userId);

            var vm = new RankingElementViewModel()
            {
                RankingElement = selectedRankingElement,
                Heading = id == 0 ? "nowa kategoria rankingu":"edycja katagorii rankingu",
                CurrentUser = currentUser
            };

            return View(vm);
        }

        #endregion

        #region edycja/dodawanie/usuwanie kategorii ------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RankingElement(RankingElement rankingElement)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            if (!ModelState.IsValid)
            {
                var vm = new RankingElementViewModel()
                {
                    RankingElement = rankingElement,
                    Heading = rankingElement.Id == 0 ?
                     "nowy element" :
                     "edycja elementu",
                    CurrentUser = currentUser
                };

                return View("RankingElement", vm);
            }

            if (rankingElement.Id == 0)
                _rankingElementService.AddRankingElement(rankingElement,userId);
            else
                _rankingElementService.UpdateRankingElement(rankingElement,userId);


            return RedirectToAction("RankingCategory", "RankingCategory",
                new { id = rankingElement.RankingCategoryId });
        }

        [HttpPost]
        public IActionResult DeleteRankingElement(int rankingCategoryId, int id)
        {
            try
            {
                var userId = User.GetUserId();
                var currentUser = _userService.GetUser(userId);
                _rankingElementService.DeleteRankingElement(rankingCategoryId, id , userId);
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
