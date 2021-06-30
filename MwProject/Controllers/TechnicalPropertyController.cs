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
    public class TechnicalPropertyController : Controller
    {

        #region konstruktor, DI
        private readonly ITechnicalPropertyService _technicalPropertyService;
        private readonly IUserService _userService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public TechnicalPropertyController(ITechnicalPropertyService technicalPropertyService, IUserService userService)
        {
            _technicalPropertyService  = technicalPropertyService;
            _userService = userService;
        }
        #endregion

        #region przeglądanie lista cech, pojedyncza cecha ---
        public IActionResult TechnicalProperties()
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var technicalProperties = _technicalPropertyService.GetTechnicalProperties();
            var vm = new TechnicalPropertiesViewModel()
            {
                TechnicalProperties = technicalProperties,
                CurrentUser = currentUser
            };

            return View(vm);
        }

        public IActionResult TechnicalProperty(int id)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var selectedProperty = id == 0 ?
                _technicalPropertyService.NewTechnicalProperty() :
                _technicalPropertyService.GetTechnicalProperty(id);

            var vm = new TechnicalPropertyViewModel()
            {
                TechnicalProperty = selectedProperty,
                Heading = "",
                CurrentUser = currentUser
            };

            return View(vm);
        }

        #endregion

        #region edycja/dodawanie/usuwanie cechy ------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TechnicalProperty(TechnicalProperty technicalProperty)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            if (!ModelState.IsValid)
            {
                var vm = new TechnicalPropertyViewModel()
                {
                    TechnicalProperty = technicalProperty,
                    Heading = technicalProperty.Id == 0 ?
                     "nowa cecha" :
                     "edycja wybranej cechy",
                    CurrentUser = currentUser
                };

                return View("TechnicalProperty", vm);
            }

            if (technicalProperty.Id == 0)
                _technicalPropertyService.AddTechnicalProperty(technicalProperty);
            else
                _technicalPropertyService.UpdateTechnicalProperty(technicalProperty);


            return RedirectToAction("TechnicalProperties", "TechnicalProperty");
        }

        [HttpPost]
        public IActionResult DeleteTechnicalProperty(int id)
        {
            try
            {
                var userId = User.GetUserId();
                var currentUser = _userService.GetUser(userId);
                _technicalPropertyService.DeleteTechnicalProperty(id);
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
                _technicalPropertyService.SetIsActiveToFalse(id);
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
