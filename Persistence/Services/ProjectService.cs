using MwProject.Core;
using MwProject.Core.Models;
using MwProject.Core.Models.Domains;
using MwProject.Core.Services;
using MwProject.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Persistence.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Project> GetProjects(ProjectsFilter projectsFilter, PagingInfo pagingInfo, int categoryId, string userId)
        {
            return _unitOfWork.Project.GetProjects(projectsFilter, pagingInfo, categoryId, userId);
        }

        public int GetNumberOfRecords(ProjectsFilter projectsFilter, int categoryId)
        {
            return _unitOfWork.Project.GetNumberOfRecords(projectsFilter, categoryId);
        }

        public IEnumerable<Category> GetUsedCategories()
        {
            return _unitOfWork.Project.GetUsedCategories();
        }

        public Project GetProject(int id, string userId)
        {
            return _unitOfWork.Project.GetProject(id, userId);
        }

        public void AddProject(Project project)
        {
            _unitOfWork.Project.AddProject(project);
            _unitOfWork.Complete();
            _unitOfWork.Project.AddTechnicalPropertiesToProject(project);
            _unitOfWork.Complete();
        }

        public void UpdateProject(Project project, string userId)
        {
            _unitOfWork.Project.UpdateProject(project, userId);
            // metoda wysyłająca maila
            // ...
            // dodatkowa modyfikacja danych
            _unitOfWork.Complete();
        }

         public void DeleteProject(int id, string userId)
        {
            _unitOfWork.Project.DeleteProject(id,userId);
            _unitOfWork.Complete();
        }

        public void FinishProject(int id, string userId)
        {
            _unitOfWork.Project.FinishProject(id, userId);
            _unitOfWork.Complete();
        }

        public Project NewProject(string userId)
        {
            return _unitOfWork.Project.NewProject(userId);
        }

        public void AddTechnicalPropertiesToProject(Project project)
        {
            _unitOfWork.Project.AddTechnicalPropertiesToProject(project);
            _unitOfWork.Complete();
        }

        public void AddQualityRequirementsToProject(Project project)
        {
            _unitOfWork.Project.AddQualityRequirementsToProject(project);
            _unitOfWork.Complete();
        }

        public void AddEconomicRequirementsToProject(Project project)
        {
            _unitOfWork.Project.AddEconomicRequirementsToProject(project);
            _unitOfWork.Complete();
        }

        public void AcceptProject(int id, string userId)
        {
            _unitOfWork.Project.AcceptProject(id, userId);
            _unitOfWork.Complete();
        }

        public void WithdrawProjectAcceptance(int id, string userId)
        {
            _unitOfWork.Project.WithdrawProjectAcceptance(id, userId);
            _unitOfWork.Complete();
        }

        public void ConfirmCalculation(int id, string userId)
        {
            _unitOfWork.Project.ConfirmCalculation(id, userId);
            _unitOfWork.Complete();
        }

        public void WithdrawConfirmationOfCalculation(int id, string userId)
        {
            _unitOfWork.Project.WithdrawConfirmationOfCalculation(id, userId);
            _unitOfWork.Complete();
        }

        public void ConfirmEstimatedSales(int id, string userId)
        {
            _unitOfWork.Project.ConfirmEstimatedSales(id, userId);
            _unitOfWork.Complete();
        }

        public void WithdrawConfirmationOfEstimatedSales(int id, string userId)
        {
            _unitOfWork.Project.WithdrawConfirmationOfEstimatedSales(id, userId);
            _unitOfWork.Complete();
        }

        public void ConfirmProject(int id, string userId)
        {

            _unitOfWork.Project.ConfirmProject(id, userId);
            _unitOfWork.Complete();
        }

        public void WithdrawProjectConfimration(int id, string userId)
        {
            _unitOfWork.Project.WithdrawProjectConfimration(id, userId);
            _unitOfWork.Complete();
        }

        public void ConfirmQualityRequirements(int id, string userId)
        {
            _unitOfWork.Project.ConfirmQualityRequirements(id, userId);
            _unitOfWork.Complete();
        }

        public void WithdrawConfirmationOfQualityRequirements(int id, string userId)
        {
            _unitOfWork.Project.WithdrawConfirmationOfQualityRequirements(id, userId);
            _unitOfWork.Complete();
        }

        public void ConfirmEconomicRequirements(int id, string userId)
        {
            _unitOfWork.Project.ConfirmEconomicRequirements(id, userId);
            _unitOfWork.Complete();
        }

        public void WithdrawConfirmationOfEconomicRequirements(int id, string userId)
        {
            _unitOfWork.Project.WithdrawConfirmationOfEconomicRequirements(id, userId);
            _unitOfWork.Complete();
        }

        public void ConfirmTechnicalProperties(int id, string userId)
        {
            _unitOfWork.Project.ConfirmTechnicalProperties(id, userId);
            _unitOfWork.Complete();
        }

        public void WithdrawConfirmationOfTechnicalProperties(int id, string userId)
        {
            _unitOfWork.Project.WithdrawConfirmationOfTechnicalProperties(id, userId);
            _unitOfWork.Complete();
        }
    }
}
