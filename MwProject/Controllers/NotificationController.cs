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
    public class NotificationController : Controller
    {

        #region konstruktor, DI
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public NotificationController(IProjectService projectService, IUserService userService, INotificationService notificationService)
        {
            _projectService = projectService;
            _userService = userService;
            _notificationService = notificationService;
        }

        #endregion

        #region przeglądanie lista kategorii, pojedyncza kategoria ---
        public IActionResult Notifications()
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var notifications = _notificationService.GetNotifications();

            var vm = new NotificationsViewModel()
            {
                Notifications = notifications,
                CurrentUser = currentUser
            };

            return View(vm);
        }

        public IActionResult Notification(int id)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            var selectedNotification = id == 0 ?
                _notificationService.NewNotification():
                _notificationService.GetNotification(id);

            var vm = new NotificationViewModel()
            {
                Notification = selectedNotification,
                Heading = "",
                CurrentUser = currentUser
            };

            return View(vm);
        }

        #endregion

        #region edycja/dodawanie/usuwanie kategorii ------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Notification(Notification notification)
        {
            try
            {
                var userId = User.GetUserId();
                var currentUser = _userService.GetUser(userId);


                if (notification.Id == 0)
                    _notificationService.AddNotification(notification);
                else
                    _notificationService.UpdateNotification(notification);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult DeleteNotification(int id)
        {
            try
            {
                var userId = User.GetUserId();
                var currentUser = _userService.GetUser(userId);
                _notificationService.DeleteNotification(id);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message });
            }

            return Json(new { success = true });
        }


        [HttpPost]
        public IActionResult ConfirmProjectNotification(int projectId, int id)
        {
            try
            {
                var userId = User.GetUserId();
                var currentUser = _userService.GetUser(userId);
                _notificationService.ConfirmProjectNotification(projectId,id,userId);
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
