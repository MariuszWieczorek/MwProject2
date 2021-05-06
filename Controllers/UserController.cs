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
    public class UserController : Controller
    {
        
        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        private readonly IUserService _userService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region lista użytkowników, pojedynczy użytkownik
        public IActionResult Users()
        {
            var userId = User.GetUserId();
            var users = _userService.GetUsers();

            var vm = new UsersViewModel()
            {
                Users = users
            };

            return View(vm);
        }

        public IActionResult SingleUser(string id)
        {
            var userId = User.GetUserId();
            var selectedUser = _userService.GetUser(id);

            var vm = new UserViewModel()
            {
                User = selectedUser,
                Heading = ""
            };

            return View(vm);
        }

        #endregion

        #region aktualizacja, usuwanie

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SingleUser(ApplicationUser user)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = new UserViewModel()
                {
                    User = user,
                    Heading = user.Id == null ?
                     "nowa użytkownik" :
                     "edycja użytkownika"
                };

                return View("SingleUser", vm);
            }

            if (user.Id != null)
                _userService.UpdateUser(user);


            return RedirectToAction("Users", "User");
        }

        #endregion



    }
}
