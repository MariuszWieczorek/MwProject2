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
    public class ProjectTechnicalPropertyController : Controller
    {
        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        private readonly IProjectTechnicalPropertyService _projectTechnicalPropertyService;
        private readonly ITechnicalPropertyService _technicalPropertyService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public ProjectTechnicalPropertyController(IProjectTechnicalPropertyService projectTechnicalPropertyService,
                                            ITechnicalPropertyService technicalPropertyService)
        {
            _projectTechnicalPropertyService = projectTechnicalPropertyService;
            _technicalPropertyService = technicalPropertyService;
        }

        // wyświetlamy wybraną kalkulację lub pusty objekt
        public IActionResult ProjectTechnicalProperty(int projectId, int id)
        {
            var userId = User.GetUserId();
            var selectedProjectTechnicalProperty = id == 0 ?
                _projectTechnicalPropertyService.NewProjectTechnicalProperty(projectId,userId) :
                _projectTechnicalPropertyService.GetProjectTechnicalProperty(projectId,id,userId);
            
            var vm = new ProjectTechnicalPropertyViewModel()
            {
                ProjectTechnicalProperty = selectedProjectTechnicalProperty,
                Heading = id == 0 ? $"nowa {projectId}" : $"edycja {projectId}",
                TechnicalProperties = _technicalPropertyService.GetTechnicalProperties()
            };
            
             return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectTechnicalProperty(ProjectTechnicalPropertyViewModel selectedProjectTechnicalProperty)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = new ProjectTechnicalPropertyViewModel()
                {
                    ProjectTechnicalProperty = selectedProjectTechnicalProperty.ProjectTechnicalProperty,
                    Heading = selectedProjectTechnicalProperty.ProjectTechnicalProperty.Id == 0 ? "nowa" : "edycja",
                    TechnicalProperties = _technicalPropertyService.GetTechnicalProperties()
                };

                return View("ProjectTechnicalProperty", vm);
            }

            if (selectedProjectTechnicalProperty.ProjectTechnicalProperty.Id == 0)
                _projectTechnicalPropertyService.AddProjectTechnicalProperty(selectedProjectTechnicalProperty.ProjectTechnicalProperty, userId);
            else
                _projectTechnicalPropertyService.UpdateProjectTechnicalProperty(selectedProjectTechnicalProperty.ProjectTechnicalProperty, userId);


            return RedirectToAction("Project", "Project",
                new { id = selectedProjectTechnicalProperty.ProjectTechnicalProperty.ProjectId });

        }


        [HttpPost]
        public IActionResult DeleteProjectTechnicalProperty(int projectId, int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectTechnicalPropertyService.DeleteProjectTechnicalProperty(projectId, id, userId);
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
