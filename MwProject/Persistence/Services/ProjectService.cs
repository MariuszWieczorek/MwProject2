using ClosedXML.Excel;
using MwProject.Core;
using MwProject.Core.Models.Enums;
using MwProject.Core.Models.Domains;
using MwProject.Core.Models.Filters;
using MwProject.Core.Services;
using MwProject.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailService;
using Microsoft.AspNetCore.Mvc;

namespace MwProject.Persistence.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly IUrlHelper _urlHelper;

        public ProjectService(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            //  _urlHelper = urlHelper;
        }

        public IEnumerable<Project> GetProjects(ProjectsFilter projectsFilter, PagingInfo pagingInfo, int categoryId, string userId)
        {
            return _unitOfWork.Project.GetProjects(projectsFilter, pagingInfo, categoryId, userId);
        }

        public int GetNumberOfRecords(ProjectsFilter projectsFilter, int categoryId, string userId)
        {
            return _unitOfWork.Project.GetNumberOfRecords(projectsFilter, categoryId, userId);
        }

        public IEnumerable<Category> GetUsedCategories()
        {
            return _unitOfWork.Project.GetUsedCategories();
        }

        public Project GetProject(int id, string userId)
        {
            return _unitOfWork.Project.GetProject(id, userId);
        }

        public void AddProject(Project project, string userId)
        {
            _unitOfWork.Project.AddProject(project);

            _unitOfWork.Complete();

            _unitOfWork.Project.AddTechnicalPropertiesToProject(project);

            _unitOfWork.Project.AddEconomicRequirementsToProject(project);

            _unitOfWork.Project.AddGeneralRequirementsToProject(project);

            _unitOfWork.Project.AddQualityRequirementsToProject(project);

            _unitOfWork.Complete();

          
            // notifications
            var id = project.Id;

            

            var allUsers = _unitOfWork.UserRepository
                .GetUsers(new UsersFilter(), new PagingInfo())
                .ToList();

            var currentUser = allUsers
                .Single(x => x.Id == userId);

            
            var usersToNotifications = allUsers
                .Where(x => x.NewProjectEmailNotification).ToList();

            var manager = allUsers
                .SingleOrDefault(x => x.Id == currentUser.ManagerId);

            if (manager != null)
            {
                usersToNotifications.Add(manager);
            }
            

            if (usersToNotifications.Any())
            {
                foreach (var user in usersToNotifications)
                {
                    var notification = new Notification()
                    {
                        ProjectId = id,
                        UserId = user.Id,
                        TimeOfNotification = DateTime.Now,
                        TypeOfNotificationId = 1,
                        Content = $"utworzono nowy projekt id = {project.Title}"
                    };
                    _unitOfWork.NotificationRepository.AddNotification(notification);
                }
            }

            _unitOfWork.Complete();

            var projectToSend = _unitOfWork.Project.GetProject(project.Id, userId);

            var notificationsToSend = projectToSend.Notifications.Where(x => x.Confirmed == false && x.TypeOfNotificationId == 1);
            if (notificationsToSend.Any())
            {
                foreach (var notification in notificationsToSend)
                {

                    string subject = $"MWProject: {notification.TypeOfNotification.Name} / {notification.Project.Number}";
                    string message = GenerateHtml(notification);
                    var listOfEmailRecipients = new List<string>()
                {
                    $"{notification.User.Email}"
                };

                    var listOfAttachments = new List<string>();

                    var mailMessage = _emailSender.CreateMailMessage(subject, message, listOfEmailRecipients, listOfAttachments);
                    _emailSender.SendEmailAsync(mailMessage);

                }
            }

            

        }


        private string GenerateHtml(Notification notification)
        {
            var html = $"Powiadomienie programu <strong> MWProject </strong> <br />";

            html += $@"<table border=1 cellpadding=5  cellspacing=1>";

            html +=
                $@"<tr>
                    <td align=center bgcolor=lightgrey> Powiadomienie </td>
                    <td align=center bgcolor=white> <strong> {notification.TypeOfNotification.Name} </strong> </td>                    
                    
                </tr>
                ";


            html +=
                $@"<tr>
                    <td align=center bgcolor=lightgrey>Tytuł</td>
                    <td align=center bgcolor=white> {notification?.Project?.Title}</td>                    
                    
                </tr>
                ";


            html +=
                $@"<tr>
                    <td align=center bgcolor=lightgrey>Numer</td>                    
                    <td align=center bgcolor=white> {notification?.Project?.Number}</td>
                </tr>
                ";


            html +=
                $@"<tr>
                    <td align=center bgcolor=lightgrey>Wnioskujący</td>                    
                    <td align=center bgcolor=white> {notification?.Project?.User?.Email}</td>
                </tr>
                ";

            //var Url = Microsoft.AspNetCore.Html.ActionLink("Project", "Project", new { id = notification.Project.Id });


            // var link = _urlHelper.ActionLink("Project", "Project", new { id = notification.Project.Id });
            var link = $@"http://192.168.1.186/mwproject/Project/Project/{notification.Project.Id}";

            html +=
                $@"<tr>
                    <td align=center bgcolor=lightgrey>Link</td>                    
                    <td align=center bgcolor=white>
                    {link}
                    </td>
                </tr>
                ";


            html += $@"</table> <br /> <br /> <i>Automatyczna wiadomość wysłana z aplikacji MWProject</i>";

            return html;
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


        public void UpdateProjectCard(Project project, string userId)
        {
            _unitOfWork.Project.UpdateProjectCard(project, userId);

            // metoda wysyłająca maila
            // ...
            // dodatkowa modyfikacja danych
            _unitOfWork.Complete();
            this.CalculatePriorityOfProject(project.Id, userId);
        }

        public void UpdateProjectPriority(Project project, string userId)
        {
            _unitOfWork.Project.UpdateProjectPriority(project, userId);
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
            int rankingOfViability = 0;
            int rankingOfCompetitiveness = 0;
            int rankingOfPurpose = 0;
            int rankingOfEstimatedPaybackTimeInMonths = 0;
            int rankingOfReturnOnInvestment = 0;
            decimal returnOnInvestment = 0M;
            int calculatedPriorityOfProject = 0;
            int rankingOfUsedProductionCapability = 0;
            decimal percentageOfUsedProductionCapability = 0M;
            int estimatedPaybackTimeInMonths = 0;
            //decimal estimatedCostOfProject = selectedProject.EstimatedCostOfProject;

            decimal estimatedCostOfProject = selectedProject.ProjectRequirements
                .Where(x => x.Requirement.Type == (int)RequirementType.Economic)
                .Sum(x => x.Value);


            decimal firstYearOfSalesValue = 0M;
            decimal firstYearOfSalesPrice = 0M;
            decimal firstYearOfSalesQty = 0M;
            decimal manufacturingCost = 0M;
            int rankingOfImplementationTimeInMonths = 0;
            int implementationTimeInMonths = 0;

            rankingOfViability = selectedProject.ViabilityOfTheProjectId == null ?
                0 : selectedProject.ViabilityOfTheProject.Index;

            rankingOfCompetitiveness = selectedProject.CompetitivenessOfTheProjectId == null ?
                0 : selectedProject.CompetitivenessOfTheProject.Index;

            rankingOfPurpose = selectedProject.PurposeOfTheProjectId == null ?
                0 : selectedProject.PurposeOfTheProject.Index;


            // dane dotyczące sprzedaży w pierwszym roku
            if (selectedProject.EstimatedSalesValues.Count > 0)
            {
                var firstSalesRecord = selectedProject.EstimatedSalesValues.Min(x => x.Id);
                var firstYearOfSales = selectedProject.EstimatedSalesValues.Single(x => x.Id == firstSalesRecord);
                firstYearOfSalesValue = firstYearOfSales.Qty * firstYearOfSales.Price;
                firstYearOfSalesPrice = firstYearOfSales.Price;
                firstYearOfSalesQty = firstYearOfSales.Qty;
            }

            // dane dotyczące kosztów
            if (selectedProject.Calculations.Any())
            {
                var firstCalculationRecord = selectedProject.Calculations.Min(x => x.Id);
                var firstCalcutation = selectedProject.Calculations.Single(x => x.Id == firstCalculationRecord);
                manufacturingCost = firstCalcutation.Ckw;
            }



            // SCZ Szacowany czas zwrotu z inwestycji (miesiące)
            if (firstYearOfSalesValue > 0)
            {
                estimatedPaybackTimeInMonths = (int)Math.Ceiling((estimatedCostOfProject / firstYearOfSalesValue) * 12);

                rankingOfEstimatedPaybackTimeInMonths = _unitOfWork.RankingCategoryRepository.GetRankingCategories()
                    .Single(x => x.Id == (int)RankingType.EstimatedPaybackTime)
                    .RankingElements
                    .Single(x => estimatedPaybackTimeInMonths >= x.RangeFrom && estimatedPaybackTimeInMonths < x.RangeTo).Index;
            }

            // ROI Return On Investment
            if (firstYearOfSalesValue > 0)
            {
                decimal estimatedProfit = (firstYearOfSalesPrice - manufacturingCost) * firstYearOfSalesQty;
                if (estimatedProfit > 0)
                {
                    returnOnInvestment = (int)Math.Ceiling((estimatedCostOfProject / estimatedProfit) * 12);
                    rankingOfReturnOnInvestment = _unitOfWork.RankingCategoryRepository.GetRankingCategories()
                    .Single(x => x.Id == (int)RankingType.ReturnOnInvestment)
                    .RankingElements
                    .Single(x => returnOnInvestment >= x.RangeFrom && returnOnInvestment < x.RangeTo).Index;
                }
            }

            // Czas implementacji

            DateTime? startDateOfTheProject = selectedProject.RealStartDateOfTheProject != null ?
                selectedProject.RealStartDateOfTheProject :
                selectedProject.PlannedStartDateOfTheProject;

            DateTime? endDateOfTheProject = selectedProject.PlannedEndDateOfTheProject;

            if (startDateOfTheProject != null && endDateOfTheProject != null)
            {
                TimeSpan interval = (DateTime)endDateOfTheProject - (DateTime)startDateOfTheProject;
                implementationTimeInMonths = (int)Math.Ceiling(interval.TotalDays / 30);

                if (implementationTimeInMonths > 0)
                {
                    rankingOfImplementationTimeInMonths = _unitOfWork.RankingCategoryRepository.GetRankingCategories()
                    .Single(x => x.Id == (int)RankingType.ImplementationTime)
                    .RankingElements
                    .Single(x => implementationTimeInMonths >= x.RangeFrom && implementationTimeInMonths < x.RangeTo).Index;
                }

            }



            // WZP - Wykorzystanie zdolności produkcyjnych
            int currentProductionVolume = selectedProject.ProductionCapacity;
            int plannedProductionVolume = selectedProject.PlannedProductionVolume;

            if (plannedProductionVolume != 0 && firstYearOfSalesQty != 0)
                percentageOfUsedProductionCapability = (decimal)currentProductionVolume / plannedProductionVolume;
            else
                percentageOfUsedProductionCapability = 0;

            rankingOfUsedProductionCapability = _unitOfWork.RankingCategoryRepository.GetRankingCategories()
            .Single(x => x.Id == (int)RankingType.UsedProductionCapability)
            .RankingElements
            .Single(x => percentageOfUsedProductionCapability >= x.RangeFrom && percentageOfUsedProductionCapability < x.RangeTo).Index;



            // Wyliczenie Priorytetu
            calculatedPriorityOfProject =
                  rankingOfViability
                + rankingOfCompetitiveness
                + rankingOfPurpose
                + rankingOfEstimatedPaybackTimeInMonths
                + rankingOfImplementationTimeInMonths
                + rankingOfUsedProductionCapability
                + rankingOfReturnOnInvestment;

            selectedProject.PriorityOfProject = calculatedPriorityOfProject;
            selectedProject.EstimatedPaybackTimeInMonths = estimatedPaybackTimeInMonths;
            selectedProject.RankingOfEstimatedPaybackTimeInMonths = rankingOfEstimatedPaybackTimeInMonths;
            selectedProject.ImplementationTimeInMonths = implementationTimeInMonths;
            selectedProject.RankingOfImplementationTimeInMonths = rankingOfImplementationTimeInMonths;
            selectedProject.ReturnOnInvestment = returnOnInvestment;
            selectedProject.RankingOfReturnOnInvestment = rankingOfReturnOnInvestment;
            selectedProject.RankingOfUsedProductionCapability = rankingOfUsedProductionCapability;
            selectedProject.PercentageOfUsedProductionCapability = percentageOfUsedProductionCapability;

            _unitOfWork.Complete();
        }

        public void CalculatePriorities(string userId)
        {
            var projects = _unitOfWork.Project.GetAllProjects(userId);
            foreach (var project in projects)
            {
                this.CalculatePriorityOfProject(project.Id, userId);
            }
        }

        public void AddNewTechnicalProperitiesAndRequirementsToAllProjects(string userId)
        {
            var projects = _unitOfWork.Project.GetAllProjects(userId);
            foreach (var project in projects)
            {
                if (project.CategoryId == 1)
                {
                    AddEconomicRequirementsToProject(project);
                    AddQualityRequirementsToProject(project);
                    AddGeneralRequirementsToProject(project);
                    AddTechnicalPropertiesToProject(project);
                }
            }
        }

        public void ExportProjectsToExcel(IEnumerable<Project> projects)
        {
            // PM> Install-Package ClosedXML

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sample Sheet");

                int row = 1;
                foreach (var project in projects)
                {
                    worksheet.Cell($"A{row}").Value = row;
                    worksheet.Cell(row, "B").Value = project.Number;
                    worksheet.Cell(row, 3).RichText.AddText(project.Title).SetFontColor(XLColor.Blue).SetBold();


                    // Add the text parts
                    /*
                    var cell = worksheet.Cell(row, "D");
                    cell.RichText
                      .AddText("Hello").SetFontColor(XLColor.Red)
                      .AddText(" BIG ").SetFontColor(XLColor.Blue).SetBold()
                      .AddText("World").SetFontColor(XLColor.Red);
                    */
                    row++;
                }

                // worksheet.Cell($"A{row}").FormulaA1 = "=MID(A1, 7, 5)";
                workbook.SaveAs("ListOfProjects.xlsx");
            }
        }

        public void ConfirmGeneralRequirements(int id, string userId)
        {
            _unitOfWork.Project.ConfirmGeneralRequirements(id, userId);
            _unitOfWork.Complete();
        }

        public void WithdrawConfirmationOfGeneralRequirements(int id, string userId)
        {
            _unitOfWork.Project.WithdrawConfirmationOfGeneralRequirements(id, userId);
            _unitOfWork.Complete();
        }

        public void AddGeneralRequirementsToProject(Project project)
        {
            _unitOfWork.Project.AddGeneralRequirementsToProject(project);
            _unitOfWork.Complete();
        }

        public void ConfirmProjectTeam(int id, string userId)
        {
            _unitOfWork.Project.ConfirmProjectTeam(id, userId);
            _unitOfWork.Complete();
        }

        public void WithdrawConfirmationOfProjectTeam(int id, string userId)
        {
            _unitOfWork.Project.WithdrawConfirmationOfProjectTeam(id, userId);
            _unitOfWork.Complete();
        }

        public int NewRawNumber(int projectCategory, DateTime? createdDate)
        {
            return _unitOfWork.Project.NewRawNumber(projectCategory, createdDate);
        }

        public (int, string) NewFullNumber(int projectCategory, DateTime? createdDate)
        {
            return _unitOfWork.Project.NewFullNumber(projectCategory, createdDate);
        }

        public int GetPageNumber(ProjectsFilter projectFilter, int categoryId, string userId, int itemPerPage, int projectId)
        {
            return _unitOfWork.Project.GetPageNumber(projectFilter, categoryId, userId, itemPerPage, projectId);
        }

        public void ConfirmRequest(int id, string userId)
        {
            _unitOfWork.Project.ConfirmRequest(id, userId);
        }

        public void WithdrawRequestConfimration(int id, string userId)
        {
            _unitOfWork.Project.WithdrawRequestConfimration(id, userId);
        }
    }
}
