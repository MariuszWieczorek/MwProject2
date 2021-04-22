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
    public class ProjectRequirementController : Controller
    {
        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        private readonly IProjectRequirementService _projectRequirementService;
        private readonly IRequirementService _requirementService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public ProjectRequirementController(IProjectRequirementService projectRequirementService,
                                            IRequirementService requirementService)
        {
            _projectRequirementService = projectRequirementService;
            _requirementService = requirementService;
        }

        // wyświetlamy wybraną kalkulację lub pusty objekt
        public IActionResult ProjectRequirement(int projectId, int id, int type)
        {
            var userId = User.GetUserId();
            var selectedProjectRequirement = id == 0 ?
                _projectRequirementService.NewProjectRequirement(projectId,userId) :
                _projectRequirementService.GetProjectRequirement(projectId,id,userId);
            
            var vm = new ProjectRequirementViewModel()
            {
                ProjectRequirement = selectedProjectRequirement,
                Heading = id == 0 ? $"nowa {projectId}" : $"edycja {projectId}",
                Requirements = _requirementService.GetRequirements()
            };
            
            if(type != 0)
            {
                vm.Requirements = _requirementService.GetRequirements().Where(x => x.Type == type);
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectRequirement(ProjectRequirementViewModel selectedProjectRequirement)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = new ProjectRequirementViewModel()
                {
                    ProjectRequirement = selectedProjectRequirement.ProjectRequirement,
                    Heading = selectedProjectRequirement.ProjectRequirement.Id == 0 ? "nowa" : "edycja",
                    Requirements = _requirementService.GetRequirements()
                };

                return View("ProjectRequirement", vm);
            }

            if (selectedProjectRequirement.ProjectRequirement.Id == 0)
                _projectRequirementService.AddProjectRequirement(selectedProjectRequirement.ProjectRequirement, userId);
            else
                _projectRequirementService.UpdateProjectRequirement(selectedProjectRequirement.ProjectRequirement, userId);


            return RedirectToAction("Project", "Project",
                new { id = selectedProjectRequirement.ProjectRequirement.ProjectId });

        }


        [HttpPost]
        public IActionResult DeleteProjectRequirement(int projectId, int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectRequirementService.DeleteProjectRequirement(projectId, id, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message });
            }

            return Json(new { success = true });
        }


    }
}
