using MwProject.Core.Models;
using MwProject.Core.Models.Domains;
using MwProject.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Services
{
    public interface IProjectService
    {
        IEnumerable<Project> GetProjects(ProjectsFilter projectsFilter, PagingInfo pagingInfo, int categoryId, string userId);
        Project GetProject(int id, string userId);
        Project NewProject(string userId);
        void AddProject(Project project);
        void UpdateProject(Project project, string userId);
        void FinishProject(int id, string userId);
        void DeleteProject(int id, string userId);
        int GetNumberOfRecords(ProjectsFilter projectFilter, int categoryId, string userId);
        void AddTechnicalPropertiesToProject(Project project);
        void AddQualityRequirementsToProject(Project project);
        void AddGeneralRequirementsToProject(Project project);
        void AddEconomicRequirementsToProject(Project project);
        void AcceptProject(int id, string userId);
        void WithdrawProjectAcceptance(int id, string userId);
        void ConfirmProject(int id, string userId);
        void WithdrawProjectConfimration(int id, string userId);
        void ConfirmCalculation(int id, string userId);
        void WithdrawConfirmationOfCalculation(int id, string userId);
        void ConfirmEstimatedSales(int id, string userId);
        void WithdrawConfirmationOfEstimatedSales(int id, string userId);
        void ConfirmQualityRequirements(int id, string userId);
        void WithdrawConfirmationOfQualityRequirements(int id, string userId);
        void ConfirmEconomicRequirements(int id, string userId);
        void WithdrawConfirmationOfEconomicRequirements(int id, string userId);
        void ConfirmGeneralRequirements(int id, string userId);
        void WithdrawConfirmationOfGeneralRequirements(int id, string userId);
        void ConfirmTechnicalProperties(int id, string userId);
        void WithdrawConfirmationOfTechnicalProperties(int id, string userId);
        void ConfirmProjectTeam(int id, string userId);
        void WithdrawConfirmationOfProjectTeam(int id, string userId);
        void CalculatePriorityOfProject(int id, string userId);
        void CalculatePriorities(string userId);
        IEnumerable<Category> GetUsedCategories();
        void ExportProjectsToExcel(IEnumerable<Project> projects);

    }
}
