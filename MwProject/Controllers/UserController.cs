using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MwProject.Core.Models.Domains;
using MwProject.Core.Models.Filters;
using MwProject.Core.Services;
using MwProject.Core.ViewModels;
using MwProject.Helpers;
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

        #region konstruktor, DI
        private readonly IUserService _userService;
        private readonly int _itemPerPage = 10;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region lista użytkowników, pojedynczy użytkownik
        public IActionResult Users(int currentPage = 1)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            // odczytanie zapamiętanych warunków selekcji w Sesji
            UsersFilter usersFilter = HttpContext.Session.GetObjectFromJson<UsersFilter>("UsersFilter");

            int numberOfRecords = _userService.GetNumberOfRecords(usersFilter);

            var pagingInfo = new PagingInfo
            {
                CurrentPage = currentPage,
                ItemsPerPage = _itemPerPage,
                TotalItems = 0
            };

            var users = _userService.GetUsers(usersFilter, pagingInfo);



            var vm = new UsersViewModel()
            {
                Users = users,
                PagingInfo = pagingInfo,
                UsersFilter = usersFilter,
                NumberOfRecords = numberOfRecords,
                CurrentUser = currentUser
            };

            return View(vm);
        }

        // akcja wywoływana w widoku Machines po kliknięciu na submit
        // służącym do filtrowania bez przeładowywania strony

        [HttpPost]
        public IActionResult Users(UsersViewModel viewModel)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            int numberOfRecords = _userService.GetNumberOfRecords(viewModel.UsersFilter);

            var pagingInfo = new PagingInfo()
            {
                CurrentPage = 1,
                ItemsPerPage = _itemPerPage,
                TotalItems = numberOfRecords
            };


            var users = _userService.GetUsers(
                viewModel.UsersFilter,
                pagingInfo
               );

            var vm = new UsersViewModel()
            {
                UsersFilter = viewModel.UsersFilter,
                Users = users,
                NumberOfRecords = numberOfRecords,
                PagingInfo = pagingInfo,
                CurrentUser = currentUser
            };

            // zapisujemy w sesji ustawienia filtra
            // potrzebne helpery SessionHelper
            HttpContext.Session.SetObjectAsJson("UsersFilter", viewModel.UsersFilter);
            HttpContext.Session.SetString("username", "abc");

            return PartialView("_UsersTablePartial", vm);
        }

        public IActionResult SingleUser(string id)
        {
            var userId = User.GetUserId();

            var selectedUser = id == null ?
              _userService.NewUser() :
              _userService.GetUser(id);

            
            var vm = new UserViewModel()
            {
                User = selectedUser,
                Heading = "",
                Managers = _userService.GetUsers(null, null).Where(x => x.IsManager == true)
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
                     "edycja użytkownika",
                    Managers = _userService.GetUsers(null, null).Where(x => x.IsManager == true)
                };

                return View("SingleUser", vm);
            }

            if (user.Id != null)
                _userService.UpdateUser(user);


            return RedirectToAction("Users", "User");
        }

        #endregion

        #region Excel

        public string ExportUsersToExcel()
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var usersFilter = new UsersFilter();
            var pagingInfo = new PagingInfo();
            var users = _userService.GetUsers(usersFilter, pagingInfo);

            _userService.ExportUsersToExcel(users);
            return "OK";
        }

        public string ImportUsersFromExcel()
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            _userService.ImportUsersFromExcel();
            return "OK";
        }

        public string RepairUsers()
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            _userService.RepairUsers();
            return "OK";
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string id)
        {

            try
            {
                var userId = User.GetUserId();
                var currentUser = _userService.GetUser(userId);

                var claims = HttpContext.User;
                var x = await _userService.ResetPassword(id);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }

        #endregion


    }
}
