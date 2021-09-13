using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MwProject.Core.Models;
using MwProject.Core.Models.Domains;
using MwProject.Core.Models.Enums;
using MwProject.Core.Models.Filters;
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
        private readonly IProjectStatusService _projectStatusService;
        private readonly IProjectGroupService _projectGroupService;
        private readonly INotificationService _notificationService;

        private readonly int _itemPerPage = 10;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public ProjectController(IProjectService projectService,
                                 ICategoryService categoryService,
                                 IProductGroupService productGroupService,
                                 IUserService userService,
                                 IRankingCategoryService rankingCategoryService,
                                 IRankingElementService rankingElementService,
                                 IProjectStatusService projectStatusService,
                                 IProjectGroupService projectGroupService,
                                 INotificationService notificationService
                                )
        {
            _projectService = projectService;
            _categoryService = categoryService;
            _productGroupService = productGroupService;
            _userService = userService;
            _rankingCategoryService = rankingCategoryService;
            _rankingElementService = rankingElementService;
            _projectStatusService = projectStatusService;
            _projectGroupService = projectGroupService;
            _notificationService = notificationService;
        }

        #endregion

        #region Lista projektów, pełen widok, widok częściowy tylko z tabelką
        public IActionResult Projects(int currentPage = 1, int categoryId = 0, int projectId = 0)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var applicationUsers = _userService.GetUsers(null,null);
            var projectStatuses = _projectStatusService.GetProjectStatuses();
            var projectGroups = _projectGroupService.GetProjectGroups();

            //HttpContext.Session.SetInt32("age", 20);
            //HttpContext.Session.SetString("username", "abc");

            ProjectsFilter projectFilter = HttpContext.Session.GetObjectFromJson<ProjectsFilter>("ProjectsFilter");
            string a = HttpContext.Session.GetString("username");

            if (projectFilter==null)
            {
                projectFilter = new();
            }

            int numberOfRecords = _projectService.GetNumberOfRecords(projectFilter, categoryId, userId);

            if (projectId != 0)
            {
                currentPage = _projectService.GetPageNumber(projectFilter, categoryId, userId, _itemPerPage, projectId); 
            } 

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
                ApplicationUsers = applicationUsers,
                ProjectStatuses = projectStatuses,
                ProjectGroups = projectGroups,
                SelectedProjectId = projectId,
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
            var projectStatuses = _projectStatusService.GetProjectStatuses();
            var projectGroups = _projectGroupService.GetProjectGroups();

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
                CurrentUser = currentUser,
                ProjectStatuses = projectStatuses,
                ProjectGroups = projectGroups
            };

            // zapisujemy w sesji ustawienia filtra
            HttpContext.Session.SetObjectAsJson("ProjectsFilter",viewModel.ProjectsFilter);
            HttpContext.Session.SetString("username", "abc");

            return PartialView("_ProjectsTablePartial", vm);
        }



        public IActionResult ProjectsStatistics(int currentPage = 1, int categoryId = 0)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var applicationUsers = _userService.GetUsers(null, null);
            var projectStatuses = _projectStatusService.GetProjectStatuses();
            var projectGroups = _projectGroupService.GetProjectGroups();
            var projectFilter = new ProjectsFilter(); 


            int numberOfRecords = _projectService.GetNumberOfRecords(projectFilter, categoryId, userId);

            var projects = _projectService.GetProjects(
                projectFilter,
                new PagingInfo() { CurrentPage = 1, ItemsPerPage = numberOfRecords, TotalItems = numberOfRecords },
                categoryId,
                userId
                );

            int mission5Count = projects.Where(x => x.ProjectGroupId == (int)ProjectGroupX.Mission50)
                                        .Count();

            int polishAreaCount = projects.Where(x => x.ProjectGroupId == (int)ProjectGroupX.PolishArea)
                                        .Count();

            

            var vm = new ProjectsStatisticsViewModel()
            {
                Categories = _categoryService.GetCategories(),
                Projects = projects,
                TotalCount = numberOfRecords,
                Mission50Count = mission5Count,
                PolishAreaCount = polishAreaCount,
                ProjectStatuses = projectStatuses,
                ProjectGroups = projectGroups,
                Users = _userService.GetUsers(null,null)
            };

            return View(vm);
        }

        public IActionResult ProjectsCategoryCharts(int currentPage = 1, int categoryId = 0)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var applicationUsers = _userService.GetUsers(null, null);
            var projectStatuses = _projectStatusService.GetProjectStatuses();
            var projectGroups = _projectGroupService.GetProjectGroups();
            var projectFilter = new ProjectsFilter();


            int numberOfRecords = _projectService.GetNumberOfRecords(projectFilter, categoryId, userId);

            var projects = _projectService.GetProjects(
                projectFilter,
                new PagingInfo() { CurrentPage = 1, ItemsPerPage = numberOfRecords, TotalItems = numberOfRecords },
                categoryId,
                userId
                );

            int mission5Count = projects.Where(x => x.ProjectGroupId == (int)ProjectGroupX.Mission50)
                                        .Count();

            int polishAreaCount = projects.Where(x => x.ProjectGroupId == (int)ProjectGroupX.PolishArea)
                                        .Count();



            var vm = new ProjectsStatisticsViewModel()
            {
                Categories = _categoryService.GetCategories(),
                Projects = projects,
                TotalCount = numberOfRecords,
                Mission50Count = mission5Count,
                PolishAreaCount = polishAreaCount,
                ProjectStatuses = projectStatuses,
                ProjectGroups = projectGroups,
                Users = _userService.GetUsers(null, null)
            };

            return View(vm);
        }

        public IActionResult ProjectsManagerCharts(int currentPage = 1, int categoryId = 0)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var applicationUsers = _userService.GetUsers(null, null);
            var projectStatuses = _projectStatusService.GetProjectStatuses();
            var projectGroups = _projectGroupService.GetProjectGroups();
            var projectFilter = new ProjectsFilter();


            int numberOfRecords = _projectService.GetNumberOfRecords(projectFilter, categoryId, userId);

            var projects = _projectService.GetProjects(
                projectFilter,
                new PagingInfo() { CurrentPage = 1, ItemsPerPage = numberOfRecords, TotalItems = numberOfRecords },
                categoryId,
                userId
                );

            int mission5Count = projects.Where(x => x.ProjectGroupId == (int)ProjectGroupX.Mission50)
                                        .Count();

            int polishAreaCount = projects.Where(x => x.ProjectGroupId == (int)ProjectGroupX.PolishArea)
                                        .Count();



            var vm = new ProjectsStatisticsViewModel()
            {
                Categories = _categoryService.GetCategories(),
                Projects = projects,
                TotalCount = numberOfRecords,
                Mission50Count = mission5Count,
                PolishAreaCount = polishAreaCount,
                ProjectStatuses = projectStatuses,
                ProjectGroups = projectGroups,
                Users = _userService.GetUsers(null, null)
            };

            return View(vm);
        }


        public IActionResult ProjectsPriorityCharts(int currentPage = 1, int categoryId = 0)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var applicationUsers = _userService.GetUsers(null, null);
            var projectStatuses = _projectStatusService.GetProjectStatuses();
            var projectGroups = _projectGroupService.GetProjectGroups();
            var projectFilter = new ProjectsFilter();


            int numberOfRecords = _projectService.GetNumberOfRecords(projectFilter, categoryId, userId);

            var projects = _projectService.GetProjects(
                projectFilter,
                new PagingInfo() { CurrentPage = 1, ItemsPerPage = numberOfRecords, TotalItems = numberOfRecords },
                categoryId,
                userId
                );

            int mission5Count = projects.Where(x => x.ProjectGroupId == (int)ProjectGroupX.Mission50)
                                        .Count();

            int polishAreaCount = projects.Where(x => x.ProjectGroupId == (int)ProjectGroupX.PolishArea)
                                        .Count();



            var vm = new ProjectsStatisticsViewModel()
            {
                Categories = _categoryService.GetCategories(),
                Projects = projects,
                TotalCount = numberOfRecords,
                Mission50Count = mission5Count,
                PolishAreaCount = polishAreaCount,
                ProjectStatuses = projectStatuses,
                ProjectGroups = projectGroups,
                Users = _userService.GetUsers(null, null)
            };

            return View(vm);
        }
        #endregion




        #region Pojedynczy projekt
        public IActionResult Project(int id, string tab)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var rankingCategories = _rankingCategoryService.GetRankingCategories();
            var applicationUsers = _userService.GetUsers(null,null);
            var projectStatuses = _projectStatusService.GetProjectStatuses();
            var projectGroups = _projectGroupService.GetProjectGroups();

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
                      "nowy projekt" :
                     $"numer: {selectedProject.Number}",
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
                ApplicationUsers = applicationUsers,
                ProjectStatuses = projectStatuses,
                ProjectGroups = projectGroups
            };

            ViewBag.Tab = tab != null ? tab : string.Empty;

            return View(vm);
        }


        // Fragment widoku z danymi do wyliczenia priorytetu
        public IActionResult ProjectPriority(int id, string tab)
        {
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var rankingCategories = _rankingCategoryService.GetRankingCategories();
            var applicationUsers = _userService.GetUsers(null, null);

            var selectedProject = id == 0 ?
                _projectService.NewProject(userId) :
                _projectService.GetProject(id, userId);

            ViewBag.Tab = tab != null ? tab : string.Empty;

           
            //IEnumerable<ApplicationUser> ApplicationUsers = _userService

           

            var vm = new ProjectViewModel()
            {
                Project = selectedProject,
                ProductGroups = _productGroupService.GetProductGroups(),
                Categories = _categoryService.GetCategories(),
                Heading = selectedProject.Id == 0 ?
                      "Nowy Projekt" :
                     $"lp: {selectedProject.OrdinalNumber} numer: {selectedProject.Number}",
           
                CurrentUser = currentUser,
                RankingCategories = rankingCategories,
                ApplicationUsers = applicationUsers,

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
            var rankingCategories = _rankingCategoryService.GetRankingCategories();
            var applicationUsers = _userService.GetUsers(null,null);
            var projectStatuses = _projectStatusService.GetProjectStatuses();
            var projectGroups = _projectGroupService.GetProjectGroups();

            ApplicationUser acceptedBy = new();
            ApplicationUser confirmedBy = new();
            ApplicationUser calculationConfirmedBy = new();
            ApplicationUser estimatedSalesConfirmedBy = new();
            ApplicationUser generalRequirementsConfirmedBy = new();
            ApplicationUser qualityRequirementsConfirmedBy = new();
            ApplicationUser economicRequirementsConfirmedBy = new();
            ApplicationUser technicalPropertiesConfirmedBy = new();
            ApplicationUser projectManager = new();

            if (!ModelState.IsValid)
            {

                if (projectViewModel.Project.AcceptedBy != null)
                {
                    acceptedBy = _userService.GetUser(projectViewModel.Project.AcceptedBy);
                }

                if (projectViewModel.Project.ConfirmedBy != null)
                {
                    confirmedBy = _userService.GetUser(projectViewModel.Project.ConfirmedBy);
                }

                if (projectViewModel.Project.CalculationConfirmedBy != null)
                {
                    calculationConfirmedBy = _userService.GetUser(projectViewModel.Project.CalculationConfirmedBy);
                }

                if (projectViewModel.Project.EstimatedSalesConfirmedBy != null)
                {
                    estimatedSalesConfirmedBy = _userService.GetUser(projectViewModel.Project.EstimatedSalesConfirmedBy);
                }

                if (projectViewModel.Project.GeneralRequirementsConfirmedBy != null)
                {
                    generalRequirementsConfirmedBy = _userService.GetUser(projectViewModel.Project.GeneralRequirementsConfirmedBy);
                }

                if (projectViewModel.Project.QualityRequirementsConfirmedBy != null)
                {
                    qualityRequirementsConfirmedBy = _userService.GetUser(projectViewModel.Project.QualityRequirementsConfirmedBy);
                }

                if (projectViewModel.Project.EconomicRequirementsConfirmedBy != null)
                {
                    economicRequirementsConfirmedBy = _userService.GetUser(projectViewModel.Project.EconomicRequirementsConfirmedBy);
                }

                if (projectViewModel.Project.TechnicalProportiesConfirmedBy != null)
                {
                    technicalPropertiesConfirmedBy = _userService.GetUser(projectViewModel.Project.TechnicalProportiesConfirmedBy);
                }

                if (projectViewModel.Project.ProjectManagerId != null)
                {
                    projectManager = _userService.GetUser(projectViewModel.Project.ProjectManagerId);
                }

                var vm = new ProjectViewModel()
                {
                    Project = projectViewModel.Project,
                    Categories = _categoryService.GetCategories(),
                    ProductGroups = _productGroupService.GetProductGroups(),
                    RankingCategories = rankingCategories,
                    ApplicationUsers = applicationUsers,
                    CurrentUser = currentUser,
                    Heading = projectViewModel.Project.Id == 0 ? "Nowy Projekt" : 
                        $"lp: {projectViewModel.Project.OrdinalNumber} numer: {projectViewModel.Project.Number}",
                    
                    AcceptedBy = acceptedBy,
                    ConfirmedBy = confirmedBy,
                    CalculationConfirmedBy = calculationConfirmedBy,
                    EstimatedSalesConfirmedBy = estimatedSalesConfirmedBy,
                    GeneralRequirementsConfirmedBy = generalRequirementsConfirmedBy,
                    QualityRequirementsConfirmedBy = qualityRequirementsConfirmedBy,
                    EconomicRequirementsConfirmedBy = economicRequirementsConfirmedBy,
                    TechnicalPropertiesConfirmedBy = technicalPropertiesConfirmedBy,
                    ProjectManager = projectManager,
                    ProjectStatuses = projectStatuses,
                    ProjectGroups = projectGroups

                };
                
                // gdy nie przeszła walidacja wracamy do ekranu edycji
                return View("Project", vm);
            }

            // jeżeli wszystko ok to zapisujemy projekt
            if (project.Id == 0)
                _projectService.AddProject(project,userId);
            else
                 _projectService.UpdateProject(project, userId);


            return RedirectToAction("Project", "Project"
               , new { id = project.Id, tab = "home" });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProjectPriority(ProjectViewModel projectViewModel)
        {
            var project = projectViewModel.Project;
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var rankingCategories = _rankingCategoryService.GetRankingCategories();
            var applicationUsers = _userService.GetUsers(null, null);

            if (!ModelState.IsValid)
            {


                var vm = new ProjectViewModel()
                {
                    Project = projectViewModel.Project,
                    Categories = _categoryService.GetCategories(),
                    ProductGroups = _productGroupService.GetProductGroups(),
                    RankingCategories = rankingCategories,
                    ApplicationUsers = applicationUsers,
                    CurrentUser = currentUser,
                    Heading = projectViewModel.Project.Id == 0 ? "Nowy Projekt" :
                        $"lp: {projectViewModel.Project.OrdinalNumber} numer: {projectViewModel.Project.Number}",

                };

                // gdy nie przeszła walidacja wracamy do ekranu edycji
                return View("ProjectPriority", vm);
            }

            // jeżeli wszystko ok to zapisujemy projekt
            _projectService.UpdateProjectPriority(project, userId);

            return RedirectToAction("Project", "Project"
                ,new { id = project.Id, tab = "priority" });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProjectCard(ProjectViewModel projectViewModel)
        {
            var project = projectViewModel.Project;
            var userId = User.GetUserId();
            var currentUser = _userService.GetUser(userId);
            var rankingCategories = _rankingCategoryService.GetRankingCategories();
            var applicationUsers = _userService.GetUsers(null, null);

            if (!ModelState.IsValid)
            {


                var vm = new ProjectViewModel()
                {
                    Project = projectViewModel.Project,
                    Categories = _categoryService.GetCategories(),
                    ProductGroups = _productGroupService.GetProductGroups(),
                    RankingCategories = rankingCategories,
                    ApplicationUsers = applicationUsers,
                    CurrentUser = currentUser,
                    Heading = projectViewModel.Project.Id == 0 ? "Nowy Projekt" :
                        $"lp: {projectViewModel.Project.OrdinalNumber} numer: {projectViewModel.Project.Number}",

                };

                // gdy nie przeszła walidacja wracamy do ekranu edycji
                return View("ProjectPriority", vm);
            }

            // jeżeli wszystko ok to zapisujemy projekt
            _projectService.UpdateProjectCard(project, userId);

            return RedirectToAction("Project", "Project"
                , new { id = project.Id, tab = "projectcard" });
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
        public IActionResult AddGeneralRequirements(int projectId)
        {
            try
            {
                var userId = User.GetUserId();
                var project = _projectService.GetProject(projectId, userId);
                _projectService.AddGeneralRequirementsToProject(project);
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

        [HttpPost]
        public IActionResult ConfirmProjectTeam(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.ConfirmProjectTeam(id, userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult WithdrawConfirmationOfProjectTeam(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.WithdrawConfirmationOfProjectTeam(id, userId);
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


        public string AddNewTechnicalProperitiesAndRequirementsToAllProjects()
        {
            var userId = User.GetUserId();
            _projectService.AddNewTechnicalProperitiesAndRequirementsToAllProjects(userId);
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

        [HttpPost]
        public IActionResult NewRawNumber(int categoryId, DateTime? createdDate)
        {
            int no = 0;
            try
            {
                var userId = User.GetUserId();
                no = _projectService.NewRawNumber(categoryId, createdDate);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true, no = no });
        }

        [HttpPost]
        public IActionResult NewFullNumber(int categoryId, DateTime? createdDate)
        {
            (int,string) no = (0,string.Empty);
            try
            {
                var userId = User.GetUserId();
                no = _projectService.NewFullNumber(categoryId, createdDate);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true, no = no.Item1, number = no.Item2 });
        }

    }
}
