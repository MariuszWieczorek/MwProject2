using Microsoft.EntityFrameworkCore;
using MwProject.Core;
using MwProject.Core.Models.Enums;
using MwProject.Core.Models.Domains;
using MwProject.Core.Models.Filters;
using MwProject.Core.Repositories;
using MwProject.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {

        private readonly IApplicationDbContext _context;
        public ProjectRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public Project NewProject(string userId)
        {

            int ordinalNumber = 1;
            var user = _context.Users.Single(x => x.Id == userId);

            if (_context.Projects.Any())
            {
                ordinalNumber = _context.Projects.Max(x => x.OrdinalNumber) + 1;
            }

            int no = 1;
            if (_context.Projects.Any())
            {
                no = _context.Projects.Max(x => x.No) + 1;
            }

            return new Project()
            {
                UserId = userId,
                CreatedDate = DateTime.Now,
                Term = DateTime.Now,
                Value = 0,
                OrdinalNumber = ordinalNumber,
                No = no,
                ProjectStatusId = (int)StatusType.NewProject,
                InitiatedBy = $"{user.FirstName} {user.LastName}"
            };
        }

        public void AddProject(Project project)
        {
            int categoryId = project.CategoryId != null ? (int)project.CategoryId : 0;
            var x = NewFullNumber(categoryId, project.CreatedDate);
            project.No = x.Item1;
            project.Number = x.Item2;
            _context.Projects.Add(project);
        }


        public void DeleteProject(int id, string userId)
        {
            var projectToDelete = _context.Projects.Single(x => x.Id == id);
            _context.Projects.Remove(projectToDelete);
        }

        public void FinishProject(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id && x.UserId == userId);
            projectToUpdate.IsExecuted = true;
        }


        public Project GetProject(int id, string userId)
        {
            var user = _context.Users.Single(x => x.Id == userId);

            var project = _context.Projects
                .Include(x => x.Calculations)
                .Include(x => x.EstimatedSalesValues)
                .Include(x => x.ProjectRequirements)
                .ThenInclude(x => x.Requirement)
                .Include(x => x.ProjectTechnicalProperties)
                .ThenInclude(x => x.TechnicalProperty)
                .Include(x => x.ProductGroup)
                .Include(x => x.Category)
                .Include(x => x.ProjectStatus)
                .Include(x => x.User)
                .Include(x => x.PurposeOfTheProject)
                .Include(x => x.ViabilityOfTheProject)
                .Include(x => x.CompetitivenessOfTheProject)
                .Include(x => x.ProjectManager)
                .Include(x => x.ProjectTeamMembers)
                .ThenInclude(x => x.User)
                .Include(x => x.ProjectClients)
                .Include(x => x.ProjectRisks)
                .Single(x => x.Id == id);


            return project;
        }


        public IEnumerable<Notification> GetNotifications(int ProjectId, string userId)
        {
            var user = _context.Users.Single(x => x.Id == userId);
            var notifications = _context.Notifications
                .Where(x => x.ProjectId == ProjectId)
                .Include(x => x.TypeOfNotification)
                .OrderBy(x=>x.Id);
            
            return notifications;
        }

        private IQueryable<Project> SortProjects(IQueryable<Project> projects)
        {
            projects = projects
                    .OrderByDescending(x => x.PriorityOfProject)
                    .ThenBy(x => x.OrdinalNumber)
                    .ThenBy(x => x.Number);

            return projects;
        }


        private IQueryable<Project> FilterProjects(IQueryable<Project> projects, ProjectsFilter projectsFilter, string userId)
        {

            var user = _context.Users.Single(x => x.Id == userId);

            if (true)
            {


                if (user.CanSeeAllProject == false)
                    projects = projects.Where(x => x.UserId == userId);

                if (projectsFilter.IsExecuted == true)
                    projects = projects.Where(x => x.IsExecuted == false && x.IsCanceled == false);

                if (projectsFilter.MyProjects)
                {
                    projects = projects
                            .Where(x => 
                                  x.ProjectManagerId == userId
                               || x.UserId == userId
                               || x.ProjectTeamMembers.Any(x => x.UserId == projectsFilter.ProjectTeamMemberId)
                                );
                }



                if (projectsFilter.ShowProjectsWithNotifications == true)
                    projects = projects.Where(x => x.Notifications.Where(x => x.UserId == userId && x.Confirmed == false).Any());

                if (projectsFilter.CategoryId != 0 && projectsFilter.CategoryId != null)
                    projects = projects.Where(x => x.CategoryId == projectsFilter.CategoryId);

                if (projectsFilter.ProjectStatusId != 0 && projectsFilter.ProjectStatusId != null)
                    projects = projects.Where(x => x.ProjectStatusId == projectsFilter.ProjectStatusId);

                if (projectsFilter.ProjectGroupId != 0 && projectsFilter.ProjectGroupId != null)
                    projects = projects.Where(x => x.ProjectGroupId == projectsFilter.ProjectGroupId);


                if (projectsFilter.ordinalNumber != 0 && projectsFilter.ordinalNumber != null)
                    projects = projects.Where(x => x.OrdinalNumber == projectsFilter.ordinalNumber);

                if (!string.IsNullOrWhiteSpace(projectsFilter.Title))
                    projects = projects.Where(x => x.Title.Contains(projectsFilter.Title));

                if (!string.IsNullOrWhiteSpace(projectsFilter.Number))
                    projects = projects.Where(x => x.Number.Contains(projectsFilter.Number));

                if (!string.IsNullOrWhiteSpace(projectsFilter.Client))
                    projects = projects.Where(x => x.Client.Contains(projectsFilter.Client));

                if (!string.IsNullOrWhiteSpace(projectsFilter.ProjectManagerId))
                    projects = projects.Where(x => x.ProjectManagerId == projectsFilter.ProjectManagerId);

                if (!string.IsNullOrWhiteSpace(projectsFilter.AuthorId))
                    projects = projects.Where(x => x.UserId == projectsFilter.AuthorId);


                if (!string.IsNullOrWhiteSpace(projectsFilter.ProjectTeamMemberId))
                    projects = projects.Where(x => x.ProjectTeamMembers.Any(x => x.UserId == projectsFilter.ProjectTeamMemberId));

                if (!string.IsNullOrWhiteSpace(projectsFilter.RelatedNumbers))
                    projects = projects.Where(x => x.ProjectRequirements.Any(x => x.RelatedNumbers.Contains(projectsFilter.RelatedNumbers)));

                // pokazujemy tylko projekty dotyczące zalogowanej osoby
                // czyli gdy jest PM, autorem, lub jest na liście zespołu projektowego
                if (false)
                {
                    projects = projects.Where(x => x.ProjectManagerId == userId || x.UserId == userId);
                }

            }

            return projects;
        }

        public IEnumerable<Project> GetProjects(ProjectsFilter projectsFilter, PagingInfo pagingInfo, int categoryId, string userId)
        {


            var projects = _context.Projects
                             .AsQueryable();

               /*.Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.ProjectManager)*/

            projects = FilterProjects(projects, projectsFilter, userId);

            projects = SortProjects(projects);


            if (pagingInfo != null)
            {
                projects = projects
                    .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.ItemsPerPage)
                    .Take(pagingInfo.ItemsPerPage);
            }


            return projects
                .ToList();
        }



        public IEnumerable<Project> GetMyProjects(ProjectsFilter projectsFilter, PagingInfo pagingInfo, int categoryId, string userId)
        {


            var projects = _context.Projects
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.ProjectManager)
                .AsQueryable();

            // zamiast filtrowania projektów
            // pokazujemy tylko projekty dotyczące zalogowanej osoby
            // czyli gdy jest PM, autorem, lub jest na liście zespołu projektowego
            projects = projects
                .Where(x => x.IsExecuted == false)
                .Where(x => x.ProjectManagerId == userId
                            || x.UserId == userId
                            || x.ProjectTeamMembers.Any(x => x.UserId == projectsFilter.ProjectTeamMemberId)
            );

            projects = SortProjects(projects);


            if (pagingInfo != null)
            {
                projects = projects
                    .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.ItemsPerPage)
                    .Take(pagingInfo.ItemsPerPage);
            }


            return projects
                .ToList();
        }


        public IEnumerable<Project> GetAllProjects(string userId)
        {
            return _context.Projects.ToList();
        }

        public int GetNumberOfRecords(ProjectsFilter projectsFilter, int categoryId, string userId)
        {

            var projects = _context.Projects
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.ProjectManager)
                .Include(x => x.ProjectStatus)
                .AsQueryable();

            projects = FilterProjects(projects, projectsFilter, userId);

            return projects.Count();
        }

        public int GetPageNumber(ProjectsFilter projectsFilter, int categoryId, string userId, int itemPerPage, int projectId)
        {

            if (itemPerPage == 0 || projectId == 0)
            {
                return 1;
            }

            var projects = _context.Projects
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.ProjectManager)
                .Include(x => x.ProjectStatus)
                .AsQueryable();

            projects = FilterProjects(projects, projectsFilter, userId);
            projects = SortProjects(projects);

            int recordNumber = 0;
            foreach (var project in projects)
            {
                recordNumber++;
                if (project.Id == projectId)
                {
                    break;
                }
            }

            int pageNumber = (recordNumber / itemPerPage) + 1;

            return pageNumber;
        }

        public IEnumerable<Category> GetUsedCategories()
        {
            var categories = _context.Projects
                .Include(x => x.Category)
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x);

            return categories;
        }

        public void UpdateProject(Project project, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == project.Id);

            projectToUpdate.Title = project.Title;
            projectToUpdate.CreatedDate = project.CreatedDate;
            projectToUpdate.OrdinalNumber = project.OrdinalNumber;
            projectToUpdate.Term = project.Term;





            projectToUpdate.Description = project.Description;



            projectToUpdate.InitiatedBy = project.InitiatedBy;
            projectToUpdate.Coordinator = project.Coordinator;
            projectToUpdate.Client = project.Client;

            // projectToUpdate.ProductStatus = project.ProductStatus;

        }





        #region domyślne listy dodawane do nowego projektu
        public void AddQualityRequirementsToProject(Project project)
        {
            var category = _context.Categories
                .Include(x => x.CategoryRequirements)
                .ThenInclude(x => x.Requirement)
                .Single(x => x.Id == project.CategoryId);

            var categoryRequirements = category.CategoryRequirements.Where(x => x.Requirement.Type == 2);
            var currentRequirements = project.ProjectRequirements;

            foreach (var requirement in categoryRequirements)
            {

                bool exists = currentRequirements.Where(x => x.RequirementId == requirement.RequirementId).Any();
                if (!exists)
                {
                    var projectRequitement = new ProjectRequirement()
                    {
                        ProjectId = project.Id,
                        RequirementId = requirement.RequirementId,
                    };

                    _context.ProjectRequirements.Add(projectRequitement);
                }
            }
        }

        public void AddEconomicRequirementsToProject(Project project)
        {
            var category = _context.Categories
                .Include(x => x.CategoryRequirements)
                .ThenInclude(x => x.Requirement)
                .Single(x => x.Id == project.CategoryId);

            var categoryRequirements = category.CategoryRequirements.Where(x => x.Requirement.Type == 1);
            var currentRequirements = project.ProjectRequirements;

            foreach (var requirement in categoryRequirements)
            {
                bool exists = currentRequirements.Where(x => x.RequirementId == requirement.RequirementId).Any();
                if (!exists)
                {
                    var projectRequitement = new ProjectRequirement()
                    {
                        ProjectId = project.Id,
                        RequirementId = requirement.RequirementId,
                    };

                    _context.ProjectRequirements.Add(projectRequitement);
                }
            }
        }

        public void AddGeneralRequirementsToProject(Project project)
        {
            var category = _context.Categories
                            .Include(x => x.CategoryRequirements)
                            .ThenInclude(x => x.Requirement)
                            .Single(x => x.Id == project.CategoryId);

            var categoryRequirements = category.CategoryRequirements.Where(x => x.Requirement.Type == (int)RequirementType.General);
            var currentRequirements = project.ProjectRequirements;

            foreach (var requirement in categoryRequirements)
            {
                bool exists = currentRequirements.Where(x => x.RequirementId == requirement.RequirementId).Any();
                if (!exists)
                {
                    var projectRequitement = new ProjectRequirement()
                    {
                        ProjectId = project.Id,
                        RequirementId = requirement.RequirementId,
                    };

                    _context.ProjectRequirements.Add(projectRequitement);
                }
            }
        }

        public void AddTechnicalPropertiesToProject(Project project)
        {

            var category = _context.Categories
                .Include(x => x.CategoryTechnicalProperties)
                .ThenInclude(x => x.TechnicalProperty)
                .Single(x => x.Id == project.CategoryId);

            var currentTechnicalProperties = project.ProjectTechnicalProperties;

            foreach (var technicalProperty in category.CategoryTechnicalProperties)
            {

                bool exists = currentTechnicalProperties.Where(x => x.TechnicalPropertyId == technicalProperty.TechnicalPropertyId).Any();
                if (!exists)
                {
                    var projectTechnicalProperty = new ProjectTechnicalProperty()
                    {
                        ProjectId = project.Id,
                        TechnicalPropertyId = technicalProperty.TechnicalPropertyId,
                    };

                    _context.ProjectTechnicalProperties.Add(projectTechnicalProperty);
                }
            }

        }

        #endregion

        #region akceptacja Projektu przez szefa
        public void AcceptProject(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsAccepted = true;
            projectToUpdate.AcceptedDate = DateTime.Now;
            projectToUpdate.AcceptedBy = userId;
            projectToUpdate.ProjectStatusId = (int)StatusType.ProjectIsAccepted;
        }

        public void WithdrawProjectAcceptance(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsAccepted = false;
            projectToUpdate.AcceptedDate = null;
            projectToUpdate.AcceptedBy = null;
            projectToUpdate.ProjectStatusId = (int)StatusType.ProjectIsConfirmed;
        }

        #endregion

        #region potwierdzanie kalkulacji TKW
        public void ConfirmCalculation(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsCalculationConfirmed = true;
            projectToUpdate.CalculationConfirmedDate = DateTime.Now;
            projectToUpdate.CalculationConfirmedBy = userId;
        }

        public void WithdrawConfirmationOfCalculation(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsCalculationConfirmed = false;
            projectToUpdate.CalculationConfirmedDate = null;
            projectToUpdate.CalculationConfirmedBy = null;
        }
        #endregion 

        #region potwierdzenie Projektu przez PM
        public void ConfirmProject(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsConfirmed = true;
            projectToUpdate.ConfirmedDate = DateTime.Now;
            projectToUpdate.ConfirmedBy = userId;
            projectToUpdate.ProjectStatusId = (int)StatusType.ProjectIsConfirmed;
        }

        public void WithdrawProjectConfimration(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsConfirmed = false;
            projectToUpdate.ConfirmedDate = null;
            projectToUpdate.ConfirmedBy = null;
            projectToUpdate.ProjectStatusId = (int)StatusType.RequestIsConfirmed;
        }
        #endregion

        #region potwierdzanie informacji podstawowych
        public void ConfirmQualityRequirements(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsQualityRequirementsConfirmed = true;
            projectToUpdate.QualityRequirementsConfirmedDate = DateTime.Now;
            projectToUpdate.QualityRequirementsConfirmedBy = userId;
            projectToUpdate.ProjectStatusId = (int)StatusType.DataConfirmationInProgres;
        }

        public void WithdrawConfirmationOfQualityRequirements(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsQualityRequirementsConfirmed = false;
            projectToUpdate.QualityRequirementsConfirmedDate = null;
            projectToUpdate.QualityRequirementsConfirmedBy = null;
        }


        public void ConfirmTechnicalProperties(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsTechnicalProportiesConfirmed = true;
            projectToUpdate.TechnicalProportiesConfirmedDate = DateTime.Now;
            projectToUpdate.TechnicalProportiesConfirmedBy = userId;
            projectToUpdate.ProjectStatusId = (int)StatusType.DataConfirmationInProgres;
        }

        public void WithdrawConfirmationOfTechnicalProperties(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsTechnicalProportiesConfirmed = false;
            projectToUpdate.TechnicalProportiesConfirmedDate = null;
            projectToUpdate.TechnicalProportiesConfirmedBy = null;
        }

        public void ConfirmGeneralRequirements(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsGeneralRequirementsConfirmed = true;
            projectToUpdate.GeneralRequirementsConfirmedDate = DateTime.Now;
            projectToUpdate.GeneralRequirementsConfirmedBy = userId;
            projectToUpdate.ProjectStatusId = (int)StatusType.DataConfirmationInProgres;
        }

        public void WithdrawConfirmationOfGeneralRequirements(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsGeneralRequirementsConfirmed = false;
            projectToUpdate.GeneralRequirementsConfirmedDate = null;
            projectToUpdate.GeneralRequirementsConfirmedBy = null;
        }

        public void ConfirmEstimatedSales(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsEstimatedSalesConfirmed = true;
            projectToUpdate.EstimatedSalesConfirmedDate = DateTime.Now;
            projectToUpdate.EstimatedSalesConfirmedBy = userId;
            projectToUpdate.ProjectStatusId = (int)StatusType.DataConfirmationInProgres;
        }

        public void WithdrawConfirmationOfEstimatedSales(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsEstimatedSalesConfirmed = false;
            projectToUpdate.EstimatedSalesConfirmedDate = null;
            projectToUpdate.EstimatedSalesConfirmedBy = null;
        }

        #endregion

        #region potwierdzenie informacji ekonomicznych
        public void ConfirmEconomicRequirements(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsEconomicRequirementsConfirmed = true;
            projectToUpdate.EconomicRequirementsConfirmedDate = DateTime.Now;
            projectToUpdate.EconomicRequirementsConfirmedBy = userId;
        }

        public void WithdrawConfirmationOfEconomicRequirements(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsEconomicRequirementsConfirmed = false;
            projectToUpdate.EconomicRequirementsConfirmedDate = null;
            projectToUpdate.EconomicRequirementsConfirmedBy = null;
        }

        #endregion

        #region potwierdzanie zespołu projektowego
        public void ConfirmProjectTeam(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsProjectTeamConfirmed = true;
            projectToUpdate.ProjectTeamConfirmedDate = DateTime.Now;
            projectToUpdate.ProjectTeamConfirmedBy = userId;
        }

        public void WithdrawConfirmationOfProjectTeam(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsProjectTeamConfirmed = false;
            projectToUpdate.ProjectTeamConfirmedDate = null;
            projectToUpdate.ProjectTeamConfirmedBy = null;
        }
        #endregion

        #region numbers

        public int NewRawNumber(int categoryId, DateTime? createdDate)
        {
            int no = 1;
            if (createdDate != null)
            {
                int year = ((DateTime)createdDate).Year;
                var projectsOfSelectedCategoryIdInSelectedYear = _context.Projects
                    .Where(x => x.CreatedDate != null && ((DateTime)x.CreatedDate).Year == year && x.CategoryId == categoryId);
                if (projectsOfSelectedCategoryIdInSelectedYear.Any())
                {
                    no = projectsOfSelectedCategoryIdInSelectedYear.Max(x => x.No) + 1;
                }
            }
            return no;
        }

        public (int, string) NewFullNumber(int categoryId, DateTime? createdDate)
        {
            string number = string.Empty;

            if (createdDate == null)
            {
                return (0, number);
            }

            int no = NewRawNumber(categoryId, createdDate);
            string nox = no.ToString("D5");
            int year = ((DateTime)createdDate).Year;
            string abbrev = _context.Categories.Single(x => x.Id == categoryId).Abbrev;
            number = $"{abbrev}/{year}/{nox}";

            return (no, number);
        }

        #endregion

        #region potwierdzanie Wniosku Projektowego
        public void ConfirmRequest(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsRequestConfirmed = true;
            projectToUpdate.RequestConfirmedDate = DateTime.Now;
            projectToUpdate.RequestConfirmedBy = userId;
            projectToUpdate.ProjectStatusId = (int)StatusType.RequestIsConfirmed;
        }

        public void WithdrawRequestConfimration(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id);
            projectToUpdate.IsRequestConfirmed = false;
            projectToUpdate.RequestConfirmedDate = null;
            projectToUpdate.RequestConfirmedBy = null;
            projectToUpdate.ProjectStatusId = (int)StatusType.DataConfirmationInProgres;
        }

        #endregion


        #region aktualizacja priorytetów, karty projektowej,PM

        public void UpdateProjectCard(Project project, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == project.Id);



            projectToUpdate.LinkToPlanner = project.LinkToPlanner;


            projectToUpdate.DescriptionOfPurpose = project.DescriptionOfPurpose;
            projectToUpdate.VerificationOperations = project.VerificationOperations;
            projectToUpdate.Comment = project.Comment;

            //if (project.ProjectStatusId != 0)
              //  projectToUpdate.ProjectStatusId = project.ProjectStatusId;

            // wybór programu
            if (project.ProjectGroupId != 0)
                projectToUpdate.ProjectGroupId = project.ProjectGroupId;

        }

        public void UpdateProjectWithAdminRights(Project project, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == project.Id);
          

            if (project.ProjectStatusId != 0)
              projectToUpdate.ProjectStatusId = project.ProjectStatusId;


            if ( !string.IsNullOrEmpty(project.UserId) )
                projectToUpdate.UserId = project.UserId;

        }

        public void UpdateProjectPriority(Project project, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == project.Id);




            projectToUpdate.PurposeOfTheProjectId = project.PurposeOfTheProjectId;
            projectToUpdate.CompetitivenessOfTheProjectId = project.CompetitivenessOfTheProjectId;
            projectToUpdate.ViabilityOfTheProjectId = project.ViabilityOfTheProjectId;


            projectToUpdate.RealStartDateOfTheProject = project.RealStartDateOfTheProject;
            projectToUpdate.PlannedStartDateOfTheProject = project.PlannedStartDateOfTheProject;
            projectToUpdate.PlannedEndDateOfTheProject = project.PlannedEndDateOfTheProject;

            projectToUpdate.ProductionCapacity = project.ProductionCapacity;
            projectToUpdate.PlannedProductionVolume = project.PlannedProductionVolume;
        }

        public bool UpdateProjectManager(Project project, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == project.Id);

            if (projectToUpdate.ProjectManagerId != project.ProjectManagerId)
            {
                projectToUpdate.ProjectManagerId = project.ProjectManagerId;
                projectToUpdate.ProjectManagerSetBy = userId;
                projectToUpdate.ProjectManagerSetDate = DateTime.Now;
                return true;
            }

            return false;
        }
        public void UpdateFinancialComments(Project project, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == project.Id);


            projectToUpdate.FinancialComments = project.FinancialComments;
            projectToUpdate.FinancialNotificationBy = userId;  
            projectToUpdate.FinancialNotificationDate = DateTime.Now;

        }

        public void UpdateProjectExecution(Project project, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == project.Id);
            



            projectToUpdate.FinishedDate = project.FinishedDate;
            projectToUpdate.IsExecuted = project.IsExecuted;
            
            projectToUpdate.CanceledDate = project.CanceledDate;
            projectToUpdate.IsCanceled = project.IsCanceled;

        }


        #endregion

    }
}
