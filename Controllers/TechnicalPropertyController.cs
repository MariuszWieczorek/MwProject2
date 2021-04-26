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
        
        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        private readonly ITechnicalPropertyService _technicalPropertyService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public TechnicalPropertyController(ITechnicalPropertyService technicalPropertyService)
        {
            _technicalPropertyService  = technicalPropertyService;
        }

        #region TechnicalProperty: przeglądanie lista cech, pojedyncza cecha ---
        public IActionResult TechnicalProperties()
        {
            var userId = User.GetUserId();
            var technicalProperties = _technicalPropertyService.GetTechnicalProperties();

            return View(technicalProperties);
        }

        public IActionResult TechnicalProperty(int id)
        {
            var userId = User.GetUserId();
            var selectedProperty = id == 0 ?
                _technicalPropertyService.NewTechnicalProperty() :
                _technicalPropertyService.GetTechnicalProperty(id);

            var vm = new TechnicalPropertyViewModel()
            {
                TechnicalProperty = selectedProperty,
                Heading = ""
            };

            return View(vm);
        }

        #endregion

        #region TechnicalProperty: edycja/dodawanie/usuwanie cechy ------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TechnicalProperty(TechnicalProperty technicalProperty)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = new TechnicalPropertyViewModel()
                {
                    TechnicalProperty = technicalProperty,
                    Heading = technicalProperty.Id == 0 ?
                     "nowa cecha" :
                     "edycja wybranej cechy"
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
                _technicalPropertyService.DeleteTechnicalProperty(id);
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
