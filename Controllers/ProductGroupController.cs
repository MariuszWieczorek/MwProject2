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

        #region konstruktor, DI
        private readonly IProductGroupService _productGroupService;
        private readonly IUserService _userService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public ProductGroupController(IProductGroupService productGroupService, IUserService userService)
        {
            _productGroupService = productGroupService;
            _userService = userService;
        }
        #endregion

        #region przeglądanie lista kategorii, pojedyncza kategoria ---
        public IActionResult ProductGroups()
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var productGroups = _productGroupService.GetProductGroups();

            var vm = new ProductGroupsViewModel()
            {
                ProductGroups = productGroups,
                CurrentUser = currentUser
            };
            
            return View(vm);
        }

        public IActionResult ProductGroup(int id)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            var selectedproductGroup = id == 0 ?
                new ProductGroup { Id = 0, Name = string.Empty } :
                _productGroupService.GetProductGroup(id);

            var vm = new ProductGroupViewModel()
            {
                ProductGroup = selectedproductGroup,
                Heading = "",
                CurrentUser = currentUser
            };

            return View(vm);
        }

        #endregion

        #region edycja/dodawanie/usuwanie kategorii ------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProductGroup(ProductGroup productGroup)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            if (!ModelState.IsValid)
            {
                var vm = new ProductGroupViewModel()
                {
                    ProductGroup = productGroup,
                    Heading = productGroup.Id == 0 ?
                     "nowa grupa wyrobów" :
                     "edycja grupy wyrobów",
                    CurrentUser = currentUser
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
                var currentUser = _userService.GetUser(userId);
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
