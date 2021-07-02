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
    public class ProjectGroupController : Controller
    {

        #region konstruktor, DI
        private readonly IProjectGroupService _projectGroupService;
        private readonly IUserService _userService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public ProjectGroupController(IProjectGroupService projectGroupService, IUserService userService)
        {
            _projectGroupService = projectGroupService;
            _userService = userService;
        }

        #endregion

        #region przeglądanie lista kategorii, pojedyncza kategoria ---
        public IActionResult ProjectGroups()
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var projectGroups = _projectGroupService.GetProjectGroups();

            var vm = new ProjectGroupsViewModel()
            {
                ProjectGroups = projectGroups,
                CurrentUser = currentUser
            };

            return View(vm);
        }

        public IActionResult ProjectGroup(int id)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            var selectedProjectGroup = id == 0 ?
                _projectGroupService.NewProjectGroup():
                _projectGroupService.GetProjectGroup(id);

            var vm = new ProjectGroupViewModel()
            {
                ProjectGroup = selectedProjectGroup,
                Heading = "",
                CurrentUser = currentUser
            };

            return View(vm);
        }

        #endregion

        #region edycja/dodawanie/usuwanie kategorii ------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProjectGroup(ProjectGroup projectGroup)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            if (!ModelState.IsValid)
            {
                var vm = new ProjectGroupViewModel()
                {
                    ProjectGroup = projectGroup,
                    Heading = projectGroup.Id == 0 ?
                     "nowa grupa" :
                     "edycja grupy",
                    CurrentUser = currentUser
                };

                return View("ProjectGroup", vm);
            }

            if (projectGroup.Id == 0)
                _projectGroupService.AddProjectGroup(projectGroup);
            else
                _projectGroupService.UpdateProjectGroup(projectGroup);


            return RedirectToAction("ProjectGroups", "ProjectGroup");
        }

        [HttpPost]
        public IActionResult DeleteProjectGroup(int id)
        {
            try
            {
                var userId = User.GetUserId();
                var currentUser = _userService.GetUser(userId);
                _projectGroupService.DeleteProjectGroup(id);
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
