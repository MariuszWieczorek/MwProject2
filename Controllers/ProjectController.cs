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
    public class ProjectController : Controller
    {

        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        private readonly IProjectService _projectService;
        private readonly ICategoryService _categoryService;
        private readonly IProductGroupService _productGroupService;
        private readonly IUserService _userService;

        private readonly int _itemPerPage = 10;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public ProjectController(IProjectService projectService,
                                 ICategoryService categoryService,
                                 IProductGroupService productGroupService,
                                 IUserService userService
                                )
        {
            _projectService = projectService;
            _categoryService = categoryService;
            _productGroupService = productGroupService;
            _userService = userService;
        }

        #region Lista projektów, pojedynczy projekt
        public IActionResult Projects(int currentPage = 1, int categoryId = 0)
        {
            var userId = User.GetUserId();
            
            int numberOfRecords = _projectService.GetNumberOfRecords(new ProjectsFilter(), categoryId);
            
            var projects = _projectService.GetProjects(new ProjectsFilter(),
                new PagingInfo() { CurrentPage = currentPage, ItemsPerPage = _itemPerPage },
                categoryId,
                userId
                );

            var vm = new ProjectsViewModel()
            {
                ProjectsFilter = new ProjectsFilter(),
                Categories = _categoryService.GetCategories(),
                Projects = projects,
                PagingInfo = new PagingInfo() { CurrentPage = currentPage, ItemsPerPage = _itemPerPage, TotalItems = numberOfRecords }
            };

            return View(vm);
        }

        public IActionResult Project(int id)
        {
            var userId = User.GetUserId();
            var selectedProject = id == 0 ?
                _projectService.NewProject(userId) :
                _projectService.GetProject(id,userId);

            ApplicationUser acceptedBy = new();
            ApplicationUser calculationConfirmedBy = new();
            ApplicationUser estiamtedSalesConfirmedBy = new();

            if (selectedProject.AcceptedBy != null)
            {
                acceptedBy = _userService.GetUser(selectedProject.AcceptedBy);
            }

            if (selectedProject.CalculationConfirmedBy != null)
            {
                calculationConfirmedBy = _userService.GetUser(selectedProject.CalculationConfirmedBy);
            }

            if (selectedProject.EstimatedSalesConfirmedBy != null)
            {
                estiamtedSalesConfirmedBy = _userService.GetUser(selectedProject.EstimatedSalesConfirmedBy);
            }

            var vm = new ProjectViewModel()
            {
                Project = selectedProject,
                ProductGroups = _productGroupService.GetProductGroups(),
                Categories = _categoryService.GetCategories(),
                Heading = selectedProject.Id == 0 ?
                      "Nowy Projekt" :
                     $"Edycja Projektu: {selectedProject.Number}",
                AcceptedBy = acceptedBy,
                CalculationConfirmedBy = calculationConfirmedBy,
                EstimatedSalesConfirmedBy = estiamtedSalesConfirmedBy
            };

            return View(vm);
        }

        #endregion

        #region zapis
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Project(Project project)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = new ProjectViewModel()
                {
                    Project = project,
                    Categories = _categoryService.GetCategories(),
                    ProductGroups = _productGroupService.GetProductGroups(),
                    Heading = project.Id == 0 ?
                      "Nowy Projekt" :
                     $"Edycja Projektu: {project.Number}"
                };

                return View("Project", vm);
            }

            if (project.Id == 0)
                _projectService.AddProject(project);
            else
                _projectService.UpdateProject(project,userId);

            if (project.Id == 0)
            {

            }

            return RedirectToAction("Projects", "Project");
        }


        [HttpPost]
        public IActionResult DeleteProject(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.DeleteProject(id,userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }


        [HttpPost]
        public IActionResult AcceptProject(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.AcceptProject(id, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }


        [HttpPost]
        public IActionResult WithdrawProjectAcceptance(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.WithdrawProjectAcceptance(id, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult AddTechnicalProperties(int projectId)
        {
            try
            {
                var userId = User.GetUserId();
                var project = _projectService.GetProject(projectId, userId);
                _projectService.AddTechnicalPropertiesToProject(project);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult AddQualityRequirements(int projectId)
        {
            try
            {
                var userId = User.GetUserId();
                var project = _projectService.GetProject(projectId, userId);
                _projectService.AddQualityRequirementsToProject(project);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult AddEconomicRequirements(int projectId)
        {
            try
            {
                var userId = User.GetUserId();
                var project = _projectService.GetProject(projectId, userId);
                _projectService.AddEconomicRequirementsToProject(project);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message });
            }

            return Json(new { success = true });
        }


        [HttpPost]
        public IActionResult ConfirmCalculation(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.ConfirmCalculation(id, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult WithdrawConfirmationOfCalculation(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.WithdrawConfirmationOfCalculation(id, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }


        [HttpPost]
        public IActionResult ConfirmEstimatedSales(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.ConfirmEstimatedSales(id, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult WithdrawConfirmationOfEstimatedSales(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.WithdrawConfirmationOfEstimatedSales(id, userId);
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
