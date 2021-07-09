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
    public class ProjectRiskController : Controller
    {
        /* Członek Grupy Projektowej - ProjectRisk */
        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        #region konstruktor, DI
        private readonly IProjectRiskService _projectRiskService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public ProjectRiskController(IProjectRiskService projectRiskService,
                                            IProjectService projectService,
                                            IUserService userService)
        {
            _projectRiskService = projectRiskService;
            _projectService = projectService;
            _userService = userService;
        }

        #endregion 

        #region edycja, dodawanie nowego
        public IActionResult ProjectRisk(int projectId, int id)
        {
            var userId = User.GetUserId();
        
            var selectedProjectRisk = id == 0 ?
                _projectRiskService.NewProjectRisk(projectId, userId) :
                _projectRiskService.GetProjectRisk(projectId, id, userId);

            var vm = new ProjectRiskViewModel()
            {
                ProjectRisk = selectedProjectRisk,
                Heading = id == 0 ? $"nowy" : $"edycja",
                ApplicationUsers = _userService.GetUsers(null,null)
            };

            return View(vm);
        }
        #endregion

        #region zapis
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectRisk(ProjectRiskViewModel selectedProjectRisk)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = new ProjectRiskViewModel()
                {
                    ProjectRisk = selectedProjectRisk.ProjectRisk,
                    Heading = selectedProjectRisk.ProjectRisk.Id == 0 ? "nowa" : "edycja",
                    ApplicationUsers = _userService.GetUsers(null,null)
                };

                return View("ProjectRisk", vm);
            }

            if (selectedProjectRisk.ProjectRisk.Id == 0)
                _projectRiskService.AddProjectRisk(selectedProjectRisk.ProjectRisk, userId);
            else
                _projectRiskService.UpdateProjectRisk(selectedProjectRisk.ProjectRisk, userId);

            string tabName = "risks";
        
            return RedirectToAction("Project", "Project",
                new { id = selectedProjectRisk.ProjectRisk.ProjectId, tab = tabName });

        }
        #endregion

        #region usuwanie 

        [HttpPost]
        public IActionResult DeleteProjectRisk(int projectId, int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectRiskService.DeleteProjectRisk(projectId, id, userId);
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
