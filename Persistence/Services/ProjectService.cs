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
            this.CalculatePriorityOfProject(project.Id, userId);
        }

        public void DeleteProject(int id, string userId)
        {
            _unitOfWork.Project.DeleteProject(id, userId);
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

        public void CalculatePriorityOfProject(int id, string userId)
        {
            var selectedProject = _unitOfWork.Project.GetProject(id, userId);
            int viabilityRanking = 0;
            int competitivenessRanking = 0;
            int purposeRanking = 0;
            int calculatedPriorityOfProject = 0;
            int estimatedPaybackTimeInMonthsRanking = 0; 

            viabilityRanking = selectedProject.ViabilityOfTheProjectId == null ?
                0 : selectedProject.ViabilityOfTheProject.Index;

            competitivenessRanking = selectedProject.CompetitivenessOfTheProjectId == null ?
                0 : selectedProject.CompetitivenessOfTheProject.Index;

            purposeRanking = selectedProject.PurposeOfTheProjectId == null ?
                0 : selectedProject.PurposeOfTheProject.Index;

            // Wyliczenie czasu zwrotu
            decimal estimatedCostOfProject = selectedProject.EstimatedCostOfProject;
            decimal firstYearOfSalesValue = 0M;
            if (selectedProject.EstimatedSalesValues.Count() > 0)
            {
                var firstYear = selectedProject.EstimatedSalesValues.Min(x => x.Year);
                var firstYearOfSales = selectedProject.EstimatedSalesValues.Single(x => x.Year == firstYear);
                firstYearOfSalesValue = firstYearOfSales.Qty * firstYearOfSales.Price;
            }
            int estimatedPaybackTimeInMonths = (int) Math.Ceiling((firstYearOfSalesValue != 0 ? (estimatedCostOfProject / firstYearOfSalesValue) * 12 : 0));

            estimatedPaybackTimeInMonthsRanking = _unitOfWork.RankingCategoryRepository.GetRankingCategories()
                .Single(x => x.Id == 6)
                .RankingElements
                .Single(x => estimatedPaybackTimeInMonths >= x.RangeFrom && estimatedPaybackTimeInMonths <= x.RangeTo).Index;

            // Czas implementacji
            int implementationTimeInMonthsRanking = 0;
            int implementationTimeInMonths = 0;

            DateTime? startDateOfTheProject = selectedProject.RealStartDateOfTheProject != null ?
                selectedProject.RealStartDateOfTheProject :
                selectedProject.PlannedStartDateOfTheProject;

            DateTime? endDateOfTheProject = selectedProject.PlannedEndDateOfTheProject;

            if (startDateOfTheProject != null && endDateOfTheProject != null)
            {
                TimeSpan interval = (DateTime)endDateOfTheProject - (DateTime)startDateOfTheProject;
                implementationTimeInMonths = (int)Math.Ceiling(interval.TotalDays/30);
                implementationTimeInMonthsRanking = _unitOfWork.RankingCategoryRepository.GetRankingCategories()
                .Single(x => x.Id == 5)
                .RankingElements
                .Single(x => implementationTimeInMonths >= x.RangeFrom && implementationTimeInMonths <= x.RangeTo).Index;

            }

            // Szacowany Zysk ROI return on investment
            decimal returnOnInvestment = 0M;
            int returnOnInvestmentRanking = 0;
            
            

            if (estimatedCostOfProject != 0 && selectedProject.Calculations.Any() && selectedProject.EstimatedSalesValues.Any())
            {
                decimal estimatedProfit = 0M;
                decimal manufacturingCost = 0M;
                var firstCalculationRecord     = selectedProject.Calculations.Min(x => x.Id);
                var firstCalcutation = selectedProject.Calculations.Single(x => x.Id == firstCalculationRecord);
                manufacturingCost = firstCalcutation.Ckw;

                decimal priceInFirstYear = 0M;
                decimal qtyInFirstYear = 0M;
                var firstSalesRecord = selectedProject.EstimatedSalesValues.Min(x => x.Id);
                var firstYearOfSales = selectedProject.EstimatedSalesValues.Single(x => x.Id == firstSalesRecord);
                priceInFirstYear = firstYearOfSales.Price;
                qtyInFirstYear = firstYearOfSales.Qty;

                estimatedProfit = (priceInFirstYear - manufacturingCost) * qtyInFirstYear;

                if(estimatedProfit != 0)
                {
                    returnOnInvestment = Math.Round(estimatedCostOfProject/estimatedProfit,2);
                }

                returnOnInvestmentRanking = _unitOfWork.RankingCategoryRepository.GetRankingCategories()
                .Single(x => x.Id == 4)
                .RankingElements
                .Single(x => returnOnInvestment >= x.RangeFrom && returnOnInvestment <= x.RangeTo).Index;

            }

            // Wyliczenie Priorytetu
            calculatedPriorityOfProject = 
                  viabilityRanking
                + competitivenessRanking
                + purposeRanking
                + estimatedPaybackTimeInMonthsRanking
                + implementationTimeInMonthsRanking
                + returnOnInvestmentRanking;

            selectedProject.PriorityOfProject = calculatedPriorityOfProject;
            selectedProject.EstimatedPaybackTimeInMonths = estimatedPaybackTimeInMonths;
            selectedProject.EstimatedPaybackTimeInMonthsRanking = estimatedPaybackTimeInMonthsRanking;
            selectedProject.ImplementationTimeInMonths = implementationTimeInMonths;
            selectedProject.ImplementationTimeInMonthsRanking = implementationTimeInMonthsRanking;
            selectedProject.ReturnOnInvestment = returnOnInvestment;
            selectedProject.ReturnOnInvestmentRanking = returnOnInvestmentRanking;

            _unitOfWork.Complete();
        }

       
    }
}
