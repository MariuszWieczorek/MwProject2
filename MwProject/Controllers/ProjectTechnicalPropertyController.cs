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
        /* Obsługa */

        #region konstruktor, DI
        private readonly IProjectTechnicalPropertyService _projectTechnicalPropertyService;
        private readonly ITechnicalPropertyService _technicalPropertyService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public ProjectTechnicalPropertyController(IProjectTechnicalPropertyService projectTechnicalPropertyService,
                                            ITechnicalPropertyService technicalPropertyService,
                                            IProjectService projectService,
                                            IUserService userService)
        {
            _projectTechnicalPropertyService = projectTechnicalPropertyService;
            _technicalPropertyService = technicalPropertyService;
            _projectService = projectService;
            _userService = userService;
        }

        #endregion

        #region odczyt z okna projektu
        // wyświetlamy wybraną cechę przypisaną do projektu z okna projektu
        public IActionResult ProjectTechnicalProperty(int projectId, int id)
        {
            var userId = User.GetUserId();
            var selectedProjectTechnicalProperty = id == 0 ?
                _projectTechnicalPropertyService.NewProjectTechnicalProperty(projectId,userId) :
                _projectTechnicalPropertyService.GetProjectTechnicalProperty(projectId,id,userId);

            var typeOfRequirement = "techniczna";

            var vm = new ProjectTechnicalPropertyViewModel()
            {
                ProjectTechnicalProperty = selectedProjectTechnicalProperty,
                Heading = id == 0 ? $"nowa: informacja {typeOfRequirement}" : $"edycja: informacja {typeOfRequirement}",
                TechnicalProperties = _technicalPropertyService.GetTechnicalProperties()
            };
            
             return View(vm);
        }

        #endregion

        #region edycja, usuwanie z okna projektu

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectTechnicalProperty(ProjectTechnicalPropertyViewModel selectedProjectTechnicalProperty)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var typeOfRequirement = "techniczna";

                var vm = new ProjectTechnicalPropertyViewModel()
                {
                    ProjectTechnicalProperty = selectedProjectTechnicalProperty.ProjectTechnicalProperty,
                    Heading = selectedProjectTechnicalProperty.ProjectTechnicalProperty.Id == 0 ? $"nowa: informacja {typeOfRequirement}" : $"edycja: informacja {typeOfRequirement}",
                    TechnicalProperties = _technicalPropertyService.GetTechnicalProperties()
                };

                return View("ProjectTechnicalProperty", vm);
            }

            if (selectedProjectTechnicalProperty.ProjectTechnicalProperty.Id == 0)
                _projectTechnicalPropertyService.AddProjectTechnicalProperty(selectedProjectTechnicalProperty.ProjectTechnicalProperty, userId);
            else
                _projectTechnicalPropertyService.UpdateProjectTechnicalProperty(selectedProjectTechnicalProperty.ProjectTechnicalProperty, userId);


            return RedirectToAction("Project", "Project",
                new { id = selectedProjectTechnicalProperty.ProjectTechnicalProperty.ProjectId, tab = "technical" });

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

        #endregion

        #region odczyt z osobnego okna listy informacji i pojedynczej informacji

        // wyświetlamy cechy techniczne przypisane do projektu do edycji z osobnego okna
        public IActionResult ProjectTechnicalProperties(int projectId)
        {
            var userId = User.GetUserId();

            var currentUser = _userService.GetUser(userId);

            var selectedProject = projectId == 0 ?
                _projectService.NewProject(userId) :
                _projectService.GetProject(projectId, userId);


            ApplicationUser technicalPropertiesConfirmedBy = new();


            if (selectedProject.TechnicalProportiesConfirmedBy != null)
            {
                technicalPropertiesConfirmedBy = _userService.GetUser(selectedProject.TechnicalProportiesConfirmedBy);
            }

            var vm = new ProjectViewModel()
            {
                Project = selectedProject,
                Heading = selectedProject.Id == 0 ?
                      "Nowy Projekt" :
                     $"Edycja Projektu: {selectedProject.Number}",
                TechnicalPropertiesConfirmedBy = technicalPropertiesConfirmedBy,
                CurrentUser = currentUser
            };

            return View(vm);
        }

        // wyświetlamy wybraną cechę przypisaną do projektu z osobnego okna
        public IActionResult ProjectTechnicalProperty2(int projectId, int id)
        {
            var userId = User.GetUserId();
            var selectedProjectTechnicalProperty = id == 0 ?
                _projectTechnicalPropertyService.NewProjectTechnicalProperty(projectId, userId) :
                _projectTechnicalPropertyService.GetProjectTechnicalProperty(projectId, id, userId);

            var vm = new ProjectTechnicalPropertyViewModel()
            {
                ProjectTechnicalProperty = selectedProjectTechnicalProperty,
                Heading = id == 0 ? $"nowa {projectId}" : $"edycja informacji technicznej",
                TechnicalProperties = _technicalPropertyService.GetTechnicalProperties()
            };

            return View(vm);
        }

        #endregion

        #region zapis - wywołany z osobnego okna
        // Zapis wywołany z osobnego okna
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectTechnicalProperty2(ProjectTechnicalPropertyViewModel selectedProjectTechnicalProperty)
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

                return View("ProjectTechnicalProperty2", vm);
            }

            if (selectedProjectTechnicalProperty.ProjectTechnicalProperty.Id == 0)
                _projectTechnicalPropertyService.AddProjectTechnicalProperty(selectedProjectTechnicalProperty.ProjectTechnicalProperty, userId);
            else
                _projectTechnicalPropertyService.UpdateProjectTechnicalProperty(selectedProjectTechnicalProperty.ProjectTechnicalProperty, userId);


            return RedirectToAction("ProjectTechnicalProperties", "ProjectTechnicalProperty",
                new { projectId = selectedProjectTechnicalProperty.ProjectTechnicalProperty.ProjectId });

        }

        #endregion

    }
}
