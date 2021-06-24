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
    public class ProjectTeamMemberController : Controller
    {
        /* Członek Grupy Projektowej - ProjectTeamMember */
        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        #region konstruktor, DI
        private readonly IProjectTeamMemberService _projectTeamMemberService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public ProjectTeamMemberController(IProjectTeamMemberService projectTeamMemberService,
                                            IProjectService projectService,
                                            IUserService userService)
        {
            _projectTeamMemberService = projectTeamMemberService;
            _projectService = projectService;
            _userService = userService;
        }

        #endregion 

        #region edycja, dodawanie nowego
        public IActionResult ProjectTeamMember(int projectId, int id)
        {
            var userId = User.GetUserId();
        
            var selectedProjectTeamMember = id == 0 ?
                _projectTeamMemberService.NewProjectTeamMember(projectId, userId) :
                _projectTeamMemberService.GetProjectTeamMember(projectId, id, userId);

            var vm = new ProjectTeamMemberViewModel()
            {
                ProjectTeamMember = selectedProjectTeamMember,
                Heading = id == 0 ? $"nowy" : $"edycja",
                ApplicationUsers = _userService.GetUsers(null,null)
            };

            return View(vm);
        }
        #endregion

        #region zapis
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectTeamMember(ProjectTeamMemberViewModel selectedProjectTeamMember)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = new ProjectTeamMemberViewModel()
                {
                    ProjectTeamMember = selectedProjectTeamMember.ProjectTeamMember,
                    Heading = selectedProjectTeamMember.ProjectTeamMember.Id == 0 ? "nowa" : "edycja",
                    ApplicationUsers = _userService.GetUsers(null,null)
                };

                return View("ProjectTeamMember", vm);
            }

            if (selectedProjectTeamMember.ProjectTeamMember.Id == 0)
                _projectTeamMemberService.AddProjectTeamMember(selectedProjectTeamMember.ProjectTeamMember, userId);
            else
                _projectTeamMemberService.UpdateProjectTeamMember(selectedProjectTeamMember.ProjectTeamMember, userId);

            string tabName = "team";
        
            return RedirectToAction("Project", "Project",
                new { id = selectedProjectTeamMember.ProjectTeamMember.ProjectId, tab = tabName });

        }
        #endregion

        #region usuwanie 

        [HttpPost]
        public IActionResult DeleteProjectTeamMember(int projectId, int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectTeamMemberService.DeleteProjectTeamMember(projectId, id, userId);
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
