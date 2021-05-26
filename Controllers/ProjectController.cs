using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MwProject.Core.Models;
using MwProject.Core.Models.Domains;
using MwProject.Core.Services;
using MwProject.Core.ViewModels;
using MwProject.Helpers;
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

        #region konstruktor, mechanizm DI 

        private readonly IProjectService _projectService;
        private readonly ICategoryService _categoryService;
        private readonly IProductGroupService _productGroupService;
        private readonly IUserService _userService;
        private readonly IRankingCategoryService _rankingCategoryService;
        private readonly IRankingElementService _rankingElementService;

        private readonly int _itemPerPage = 10;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public ProjectController(IProjectService projectService,
                                 ICategoryService categoryService,
                                 IProductGroupService productGroupService,
                                 IUserService userService,
                                 IRankingCategoryService rankingCategoryService,
                                 IRankingElementService rankingElementService
                                )
        {
            _projectService = projectService;
            _categoryService = categoryService;
            _productGroupService = productGroupService;
            _userService = userService;
            _rankingCategoryService = rankingCategoryService;
            _rankingElementService = rankingElementService;
        }

        #endregion

        #region Lista projektów, pełen widok, widok częściowy tylko z tabelką
        public IActionResult Projects(int currentPage = 1, int categoryId = 0)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var applicationUsers = _userService.GetUsers();

            //HttpContext.Session.SetInt32("age", 20);
            //HttpContext.Session.SetString("username", "abc");

            ProjectsFilter projectFilter = HttpContext.Session.GetObjectFromJson<ProjectsFilter>("ProjectsFilter");
            string a = HttpContext.Session.GetString("username");

            if (projectFilter==null)
            {
                projectFilter = new();
            }

            int numberOfRecords = _projectService.GetNumberOfRecords(projectFilter, categoryId, userId);

            var projects = _projectService.GetProjects(
                projectFilter,
                new PagingInfo() { CurrentPage = currentPage, ItemsPerPage = _itemPerPage },
                categoryId,
                userId
                );

            var vm = new ProjectsViewModel()
            {
                ProjectsFilter = projectFilter,
                Categories = _categoryService.GetCategories(),
                Projects = projects,
                PagingInfo = new PagingInfo() { CurrentPage = currentPage, ItemsPerPage = _itemPerPage, TotalItems = numberOfRecords },
                CurrentUser = currentUser,
                ApplicationUsers = applicationUsers
            };

            return View(vm);
        }




        // akcja wywoływana w widoku Projects po kliknięciu na submit służącym do filtrowania zadań
        // wersja bez przeładowywania strony

        [HttpPost]
        public IActionResult Projects(ProjectsViewModel viewModel)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);

            int numberOfRecords = _projectService.GetNumberOfRecords(viewModel.ProjectsFilter, 0, userId);

            var pagingInfo = new PagingInfo() { CurrentPage = 1, ItemsPerPage = _itemPerPage, TotalItems = numberOfRecords };

            var projects = _projectService.GetProjects(
                viewModel.ProjectsFilter,
                pagingInfo,
                0,
                userId
               );

            
            var vm = new ProjectsViewModel()
            {
                ProjectsFilter = viewModel.ProjectsFilter,
                Categories = _categoryService.GetCategories(),
                Projects = projects,
                PagingInfo = pagingInfo,
                CurrentUser = currentUser
            };

            // zapisujemy w sesji ustawienia filtra
            HttpContext.Session.SetObjectAsJson("ProjectsFilter",viewModel.ProjectsFilter);
            HttpContext.Session.SetString("username", "abc");

            return PartialView("_ProjectsTablePartial", vm);
        }

        #endregion

  


        #region Pojedynczy projekt
        public IActionResult Project(int id, string tab)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var rankingCategories = _rankingCategoryService.GetRankingCategories();
            var applicationUsers = _userService.GetUsers();

            var selectedProject = id == 0 ?
                _projectService.NewProject(userId) :
                _projectService.GetProject(id,userId);

            ViewBag.Tab = tab != null ? tab : string.Empty;

            ApplicationUser acceptedBy = new();
            ApplicationUser confirmedBy = new();
            ApplicationUser calculationConfirmedBy = new();
            ApplicationUser estimatedSalesConfirmedBy = new();
            ApplicationUser generalRequirementsConfirmedBy = new();
            ApplicationUser qualityRequirementsConfirmedBy = new();
            ApplicationUser economicRequirementsConfirmedBy = new();
            ApplicationUser technicalPropertiesConfirmedBy = new();
            ApplicationUser projectManager = new();

            //IEnumerable<ApplicationUser> ApplicationUsers = _userService

            if (selectedProject.AcceptedBy != null)
            {
                acceptedBy = _userService.GetUser(selectedProject.AcceptedBy);
            }

            if (selectedProject.ConfirmedBy != null)
            {
                confirmedBy = _userService.GetUser(selectedProject.ConfirmedBy);
            }

            if (selectedProject.CalculationConfirmedBy != null)
            {
                calculationConfirmedBy = _userService.GetUser(selectedProject.CalculationConfirmedBy);
            }

            if (selectedProject.EstimatedSalesConfirmedBy != null)
            {
                estimatedSalesConfirmedBy = _userService.GetUser(selectedProject.EstimatedSalesConfirmedBy);
            }

            if (selectedProject.GeneralRequirementsConfirmedBy != null)
            {
                generalRequirementsConfirmedBy = _userService.GetUser(selectedProject.GeneralRequirementsConfirmedBy);
            }

            if (selectedProject.QualityRequirementsConfirmedBy != null)
            {
                qualityRequirementsConfirmedBy = _userService.GetUser(selectedProject.QualityRequirementsConfirmedBy);
            }

            if (selectedProject.EconomicRequirementsConfirmedBy != null)
            {
                economicRequirementsConfirmedBy = _userService.GetUser(selectedProject.EconomicRequirementsConfirmedBy);
            }

            if (selectedProject.TechnicalProportiesConfirmedBy != null)
            {
                technicalPropertiesConfirmedBy = _userService.GetUser(selectedProject.TechnicalProportiesConfirmedBy);
            }

            if (selectedProject.ProjectManagerId != null)
            {
                projectManager = _userService.GetUser(selectedProject.ProjectManagerId);
            }

            var vm = new ProjectViewModel()
            {
                Project = selectedProject,
                ProductGroups = _productGroupService.GetProductGroups(),
                Categories = _categoryService.GetCategories(),
                Heading = selectedProject.Id == 0 ?
                      "Nowy Projekt" :
                     $"lp: {selectedProject.OrdinalNumber} numer: {selectedProject.Number}",
                AcceptedBy = acceptedBy,
                ConfirmedBy = confirmedBy,
                CalculationConfirmedBy = calculationConfirmedBy,
                EstimatedSalesConfirmedBy = estimatedSalesConfirmedBy,
                GeneralRequirementsConfirmedBy = generalRequirementsConfirmedBy,
                QualityRequirementsConfirmedBy = qualityRequirementsConfirmedBy,
                EconomicRequirementsConfirmedBy = economicRequirementsConfirmedBy,
                TechnicalPropertiesConfirmedBy = technicalPropertiesConfirmedBy,
                CurrentUser = currentUser,
                ProjectManager = projectManager,
                RankingCategories = rankingCategories,
                ApplicationUsers = applicationUsers
            };

            ViewBag.Tab = tab != null ? tab : string.Empty;

            return View(vm);
        }

        #endregion

        #region update-project, delete-project
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Project(ProjectViewModel projectViewModel)
        {
            var project = projectViewModel.Project;
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);


            if (!ModelState.IsValid)
            {
               
                var vm = new ProjectViewModel()
                {
                    Project = projectViewModel.Project,
                    Categories = _categoryService.GetCategories(),
                    ProductGroups = _productGroupService.GetProductGroups(),
                    RankingCategories = projectViewModel.RankingCategories,
                    ApplicationUsers = projectViewModel.ApplicationUsers,
                    CurrentUser = currentUser,
                    Heading = projectViewModel.Project.Id == 0 ? "Nowy Projekt" : 
                        $"lp: {projectViewModel.Project.OrdinalNumber} numer: {projectViewModel.Project.Number}",
                    AcceptedBy = projectViewModel.AcceptedBy,
                    ConfirmedBy = projectViewModel.ConfirmedBy,
                    CalculationConfirmedBy = projectViewModel.CalculationConfirmedBy,
                    EstimatedSalesConfirmedBy = projectViewModel.EstimatedSalesConfirmedBy,
                    GeneralRequirementsConfirmedBy = projectViewModel.GeneralRequirementsConfirmedBy,
                    QualityRequirementsConfirmedBy = projectViewModel.QualityRequirementsConfirmedBy,
                    EconomicRequirementsConfirmedBy = projectViewModel.EconomicRequirementsConfirmedBy,
                    TechnicalPropertiesConfirmedBy = projectViewModel.TechnicalPropertiesConfirmedBy,
                    ProjectManager = projectViewModel.ProjectManager
                };
                
                // gdy nie przeszła walidacja wracamy do ekranu edycji
                return View("Project", vm);
            }

            // jeżeli wszystko ok to zapisujemy projekt
            if (project.Id == 0)
                _projectService.AddProject(project);
            else
                _projectService.UpdateProject(project,userId);
                    
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

        #endregion

        #region akceptacja, potwierdzenia, wycofywanie potwierdzeń
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


        public IActionResult ConfirmProject(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.ConfirmProject(id, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }


        [HttpPost]
        public IActionResult WithdrawProjectConfimration(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.WithdrawProjectConfimration(id, userId);
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


        [HttpPost]
        public IActionResult ConfirmQualityRequirements(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.ConfirmQualityRequirements(id, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult WithdrawConfirmationOfQualityRequirements(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.WithdrawConfirmationOfQualityRequirements(id, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }


        [HttpPost]
        public IActionResult ConfirmEconomicRequirements(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.ConfirmEconomicRequirements(id, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult WithdrawConfirmationOfEconomicRequirements(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.WithdrawConfirmationOfEconomicRequirements(id, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }


        [HttpPost]
        public IActionResult ConfirmGeneralRequirements(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.ConfirmGeneralRequirements(id, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult WithdrawConfirmationOfGeneralRequirements(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.WithdrawConfirmationOfGeneralRequirements(id, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }


        [HttpPost]
        public IActionResult ConfirmTechnicalProperties(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.ConfirmTechnicalProperties(id, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult WithdrawConfirmationOfTechnicalProperties(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.WithdrawConfirmationOfTechnicalProperties(id, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }

        #endregion

        public string CalculatePriorities()
        {
            var userId = User.GetUserId();
            _projectService.CalculatePriorities(userId);
            return "OK";
        }


        public string ExportProjectsToExcel()
        {
            var userId = User.GetUserId();
            int categoryId = 0;
            int currentPage = 1;
            var currentUser = _userService.GetUser(userId);

            var projects = _projectService.GetProjects(
                new ProjectsFilter(),
                new PagingInfo() { CurrentPage = currentPage, ItemsPerPage = _itemPerPage },
                categoryId,
                userId
                );

            _projectService.ExportProjectsToExcel(projects);
            return "OK";
        }

    }
}
