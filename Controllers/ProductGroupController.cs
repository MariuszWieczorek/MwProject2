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
    public class ProductGroupController : Controller
    {
        
        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        private readonly IProductGroupService _productGroupService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public ProductGroupController(IProductGroupService productGroupService)
        {
            _productGroupService = productGroupService;
        }

        #region Kategorie: przeglądanie lista kategorii, pojedyncza kategoria ---
        public IActionResult ProductGroups()
        {
            var userId = User.GetUserId();
            var productGroups = _productGroupService.GetProductGroups();

            return View(productGroups);
        }

        public IActionResult ProductGroup(int id)
        {
            var userId = User.GetUserId();
            var selectedproductGroup = id == 0 ?
                new ProductGroup { Id = 0, Name = string.Empty } :
                _productGroupService.GetProductGroup(id);

            var vm = new ProductGroupViewModel()
            {
                ProductGroup = selectedproductGroup,
                Heading = ""
            };

            return View(vm);
        }

        #endregion

        #region Kategorie: edycja/dodawanie/usuwanie kategorii ------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProductGroup(ProductGroup productGroup)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = new ProductGroupViewModel()
                {
                    ProductGroup = productGroup,
                    Heading = productGroup.Id == 0 ?
                     "nowa grupa wyrobów" :
                     "edycja grupy wyrobów"
                };

                return View("ProductGroup", vm);
            }

            if (productGroup.Id == 0)
                _productGroupService.AddProductGroup(productGroup);
            else
                _productGroupService.UpdateProductGroup(productGroup);


            return RedirectToAction("ProductGroups", "ProductGroup");
        }

        [HttpPost]
        public IActionResult DeleteProductGroup(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _productGroupService.DeleteProductGroup(id);
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
