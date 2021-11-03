using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MwProject.Core.Models;
using MwProject.Core.Models.Domains;
using MwProject.Core.Models.Enums;
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

        #region konstruktor, DI
        private readonly IProjectRequirementService _projectRequirementService;
        private readonly IRequirementService _requirementService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public ProjectRequirementController(IProjectRequirementService projectRequirementService,
                                            IRequirementService requirementService,
                                            IProjectService projectService,
                                            IUserService userService)
        {
            _projectRequirementService = projectRequirementService;
            _requirementService = requirementService;
            _projectService = projectService;
            _userService = userService;
        }

        #endregion 

        #region ekran edycji wymagania na liście wymagań dla projektu - wywołany z okna projektu
        // wyświetlamy widok do edycji wymagania dla projektu
        // wspólny dla wymagań ekonomicznych (type == 1) i jakościowych (type == 2)
        public IActionResult ProjectRequirement(int projectId, int id, int type)
        {
            var userId = User.GetUserId();
            string typeOfRequirement;
            string tab;
            RequirementType requirementType = RequirementType.General;

            switch (type)
            {
                case 1:
                    typeOfRequirement = "ekonomiczna";
                    tab = "economic";
                    requirementType = RequirementType.Economic;
                    break;
                case 2:
                    typeOfRequirement = "jakościowa";
                    tab = "quality";
                    requirementType = RequirementType.Quality;
                    break;
                case 3:
                    typeOfRequirement = "ogólna";
                    tab = "general";
                    requirementType = RequirementType.General;
                    break;
                default:
                    typeOfRequirement = "inna";
                    tab = "";
                    break;
            }

            var selectedProjectRequirement = id == 0 ?
                _projectRequirementService.NewProjectRequirement(projectId, userId) :
                _projectRequirementService.GetProjectRequirement(projectId, id, userId);

            var vm = new ProjectRequirementViewModel()
            {
                ProjectRequirement = selectedProjectRequirement,
                Heading = id == 0 ? $"nowa: informacja {typeOfRequirement}" : $"edycja: informacja {typeOfRequirement}",
                Requirements = _requirementService.GetRequirements(),
                RequirementType = requirementType,
                Tab = tab
            };

            if (type != 0)
            {
                vm.Requirements = _requirementService.GetRequirements().Where(x => x.Type == type);
            }

            return View(vm);
        }
        #endregion

        #region zapis - wymaganie na liście wymagań na projekcie - wywołany z okna projektu
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
                    RequirementType = selectedProjectRequirement.RequirementType,
                    Requirements = _requirementService.GetRequirements()
                    
                };

                return View("ProjectRequirement", vm);
            }

            if (selectedProjectRequirement.ProjectRequirement.Id == 0)
                _projectRequirementService.AddProjectRequirement(selectedProjectRequirement.ProjectRequirement, userId);
            else
                _projectRequirementService.UpdateProjectRequirement(selectedProjectRequirement.ProjectRequirement, userId);

            _projectService.CalculatePriorityOfProject(selectedProjectRequirement.ProjectRequirement.ProjectId, userId);

            var requirementId = selectedProjectRequirement.ProjectRequirement.RequirementId;
            var requirement = _requirementService.GetRequirement(requirementId);

            string tabName = "";
            if (requirement != null)
            {
                switch (requirement.Type)
                {
                    case 1:
                        tabName = "economic";
                        break;
                    case 2:
                        tabName = "quality";
                        break;
                    case 3:
                        tabName = "general";
                        break;
                    default:
                        tabName = "";
                        break;
                }
            }



            return RedirectToAction("Project", "Project",
                new { id = selectedProjectRequirement.ProjectRequirement.ProjectId, tab = tabName });

        }
        #endregion

        #region usuwanie wymagania z listy wymagań dla projektu

        [HttpPost]
        public IActionResult DeleteProjectRequirement(int projectId, int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectRequirementService.DeleteProjectRequirement(projectId, id, userId);
                _projectService.CalculatePriorityOfProject(projectId, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message });
            }

            return Json(new { success = true });
        }

        #endregion

        #region odczyt listy wymagań dla dla projektu - wywołany z osobnego okna

        // wyświetlamy listę wymagań przypisaną do projektu do edycji w osobnym oknie
        public IActionResult ProjectRequirements(int projectId, int type)
        {
            var userId = User.GetUserId();

            var currentUser = _userService.GetUser(userId);

            var selectedProject = projectId == 0 ?
                _projectService.NewProject(userId) :
                _projectService.GetProject(projectId, userId);


            string nameOfRequirement = type == 1 ? " informacje ekonomiczne" : " informacje jakościowe";

            ApplicationUser qualityRequirementsConfirmedBy = new();
            ApplicationUser economicRequirementsConfirmedBy = new();

            if (selectedProject.QualityRequirementsConfirmedBy != null)
            {
                qualityRequirementsConfirmedBy = _userService.GetUser(selectedProject.QualityRequirementsConfirmedBy);
            }

            if (selectedProject.EconomicRequirementsConfirmedBy != null)
            {
                economicRequirementsConfirmedBy = _userService.GetUser(selectedProject.EconomicRequirementsConfirmedBy);
            }

            var vm = new ProjectViewModel()
            {
                Project = selectedProject,
                Heading = selectedProject.Id == 0 ?
                     $"Nowy Projekt / {nameOfRequirement}" :
                     $"Edycja Projektu: {selectedProject.Number} / {nameOfRequirement}",
                QualityRequirementsConfirmedBy = qualityRequirementsConfirmedBy,
                EconomicRequirementsConfirmedBy = economicRequirementsConfirmedBy,
                CurrentUser = currentUser,
                TypeOfRequirement = type
            };

            return View(vm);
        }

        #endregion

        #region ekran edycji wymagania na liście wymagań dla projektu - wywołany z osobnego okna
        // wyświetlamy wybraną cechę przypisaną do projektu z osobnego okna

        public IActionResult ProjectRequirement2(int projectId, int id, int type)
        {
            var userId = User.GetUserId();
            var selectedProjectRequirement = id == 0 ?
                _projectRequirementService.NewProjectRequirement(projectId, userId) :
                _projectRequirementService.GetProjectRequirement(projectId, id, userId);

            string nameOfRequirement = type == 1 ? " informacja ekonomiczna" : " informacja jakościowa";

            var vm = new ProjectRequirementViewModel()
            {
                ProjectRequirement = selectedProjectRequirement,
                Heading = id == 0 ? $"nowa {nameOfRequirement}" : $"edycja {nameOfRequirement}",
                Requirements = _requirementService.GetRequirements().Where(x => x.Type == type)
            };

            return View(vm);
        }

        #endregion

        #region zapis - wymaganie na liście wymagań na projekcie - wywołany z osobnego okna

        // Zapis wywołany z osobnego okna
        // po zapisie wracamy właśnie do tego osobnego okna
        // a nie do okna projektu

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectRequirement2(ProjectRequirementViewModel selectedProjectRequirement)
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

                return View("ProjectRequirement2", vm);
            }

            if (selectedProjectRequirement.ProjectRequirement.Id == 0)
                _projectRequirementService.AddProjectRequirement(selectedProjectRequirement.ProjectRequirement, userId);
            else
                _projectRequirementService.UpdateProjectRequirement(selectedProjectRequirement.ProjectRequirement, userId);

            // ekran z listą wymagań dla projektu wspólny więc musimy podać parametr type
            // aby wyświetlić odpowiednie parametry

            return RedirectToAction("ProjectRequirements", "ProjectRequirement",
                new
                {
                    projectId = selectedProjectRequirement.ProjectRequirement.ProjectId,
                    type = selectedProjectRequirement.ProjectRequirement.Requirement.Type
                });

        }

        #endregion
    }
}
