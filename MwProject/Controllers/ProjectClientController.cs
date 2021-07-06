using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MwProject.Core.Models;
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
    public class ProjectClientController : Controller
    {
        /* Członek Grupy Projektowej - ProjectClient */
        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        #region konstruktor, DI
        private readonly IProjectClientService _projectClientService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public ProjectClientController(IProjectClientService projectClientService,
                                            IProjectService projectService,
                                            IUserService userService)
        {
            _projectClientService = projectClientService;
            _projectService = projectService;
            _userService = userService;
        }

        #endregion 

        #region edycja, dodawanie nowego
        public IActionResult ProjectClient(int projectId, int id)
        {
            var userId = User.GetUserId();
        
            var selectedProjectClient = id == 0 ?
                _projectClientService.NewProjectClient(projectId, userId) :
                _projectClientService.GetProjectClient(projectId, id, userId);

            var vm = new ProjectClientViewModel()
            {
                ProjectClient = selectedProjectClient,
                Heading = id == 0 ? $"nowy" : $"edycja",
                ApplicationUsers = _userService.GetUsers(null,null)
            };

            return View(vm);
        }
        #endregion

        #region zapis
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectClient(ProjectClientViewModel selectedProjectClient)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = new ProjectClientViewModel()
                {
                    ProjectClient = selectedProjectClient.ProjectClient,
                    Heading = selectedProjectClient.ProjectClient.Id == 0 ? "nowa" : "edycja",
                    ApplicationUsers = _userService.GetUsers(null,null)
                };

                return View("ProjectClient", vm);
            }

            if (selectedProjectClient.ProjectClient.Id == 0)
                _projectClientService.AddProjectClient(selectedProjectClient.ProjectClient, userId);
            else
                _projectClientService.UpdateProjectClient(selectedProjectClient.ProjectClient, userId);

            string tabName = "clients";
        
            return RedirectToAction("Project", "Project",
                new { id = selectedProjectClient.ProjectClient.ProjectId, tab = tabName });

        }
        #endregion

        #region usuwanie 

        [HttpPost]
        public IActionResult DeleteProjectClient(int projectId, int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectClientService.DeleteProjectClient(projectId, id, userId);
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
