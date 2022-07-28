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
using System.IO;

namespace MwProject.Persistence.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        //private readonly IUrlHelper _urlHelper;

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


            GenerateNotificationsNewRequest(project.Id, userId);

            _unitOfWork.Complete();

            SendNotifications(project.Id, 1, userId);

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



        #region wysyłanie powiadomień via e-mail
        private void SendNotifications(int projectId, int typeOfNotification, string userId)
        {
            /*
            var projectToSend = _unitOfWork.Project.GetProject(projectId, userId);
            var notificationsToSend = projectToSend.Notifications
                .Where(x => x.Confirmed == false
                && x.TypeOfNotificationId == typeOfNotification
                && x.Sent == false);
            */

            var notificationsToSend = _unitOfWork.Project
                .GetNotifications(projectId, userId)
                .Where(x => x.Confirmed == false
                && x.TypeOfNotificationId == typeOfNotification
                && x.Sent == false);

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
                    notification.Sent = true;
                    _unitOfWork.Complete();

                }
            }
        }

        #endregion

        #region generowanie powiadomień

        private void GenerateNotificationsNewRequest(int projectId, string userId)
        {

            var project = _unitOfWork.Project.GetProject(projectId, userId);

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



            // var Url = Microsoft.AspNetCore.Html.ActionLink("Project", "Project", new { id = notification.Project.Id });
            // var link = _urlHelper.ActionLink("Project", "Project", new { id = notification.Project.Id });
            string link = $@"http://192.168.1.186/mwproject/Project/Project/{project.Id}";

            if (usersToNotifications.Any())
            {
                foreach (var user in usersToNotifications)
                {
                    var notification = new Notification()
                    {
                        ProjectId = project.Id,
                        UserId = user.Id,
                        TimeOfNotification = DateTime.Now,
                        TypeOfNotificationId = 1,
                        Content = $"utworzono nowy projekt: {project.Title}",
                        Link = link,
                        ToDo = "",
                        Sent = false
                    };
                    _unitOfWork.NotificationRepository.AddNotification(notification);
                }
            }


        }
        private void GenerateNotificationsAllTabsAreConfirmed(int projectId, string userId)
        {
            var project = _unitOfWork.Project.GetProject(projectId, userId);

            var allUsers = _unitOfWork.UserRepository
                .GetUsers(new UsersFilter(), new PagingInfo())
                .ToList();

            var currentUser = allUsers
                .Single(x => x.Id == project.UserId);


            var usersToNotifications = allUsers
                .Where(x => x.NewProjectEmailNotification).ToList();

            var manager = allUsers
                .SingleOrDefault(x => x.Id == currentUser.ManagerId);

            string link = $@"http://192.168.1.186/mwproject/Project/Project/{project.Id}";

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
                        ProjectId = project.Id,
                        UserId = user.Id,
                        TimeOfNotification = DateTime.Now,
                        TypeOfNotificationId = 2,
                        Content = $"Uzupełniono wszystkie zakładki informacyjne",
                        Link = link,
                        ToDo = "Proszę potwierdzić Wniosek Projektowy",
                        Sent = false
                    };
                    _unitOfWork.NotificationRepository.AddNotification(notification);
                }
            }


            _unitOfWork.Complete();

        }


        private void GenerateNotificationsRequestIsConfirmed(int projectId, string userId)
        {
            var project = _unitOfWork.Project.GetProject(projectId, userId);

            var allUsers = _unitOfWork.UserRepository
                .GetUsers(new UsersFilter(), new PagingInfo())
                .ToList();

            var currentUser = allUsers
                .Single(x => x.Id == project.UserId);


            var usersToNotifications = allUsers
                .Where(x => x.CanSetProjectManager).ToList();



            string link = $@"http://192.168.1.186/mwproject/Project/Project/{project.Id}";

            if (usersToNotifications.Any())
            {
                foreach (var user in usersToNotifications)
                {
                    var notification = new Notification()
                    {
                        ProjectId = project.Id,
                        UserId = user.Id,
                        TimeOfNotification = DateTime.Now,
                        TypeOfNotificationId = 3,
                        Content = $"Wniosek Projektowy Został Potwierdzony",
                        Link = link,
                        ToDo = "Proszę wybrać Project Manager'a",
                        Sent = false
                    };
                    _unitOfWork.NotificationRepository.AddNotification(notification);
                }
            }


            _unitOfWork.Complete();

        }

        private void GenerateNotificationsProjectManagerIsSet(int projectId, string userId)
        {
            var project = _unitOfWork.Project.GetProject(projectId, userId);

            var allUsers = _unitOfWork.UserRepository
                .GetUsers(new UsersFilter(), new PagingInfo())
                .ToList();

            var currentUser = allUsers
                .Single(x => x.Id == project.UserId);


            var projectManager = allUsers
            .SingleOrDefault(x => x.Id == project.ProjectManagerId);

            var usersToNotifications = allUsers
                .Where(x => x.Id == project.ProjectManagerId).ToList();



            string link = $@"http://192.168.1.186/mwproject/Project/Project/{project.Id}";

            if (usersToNotifications.Any())
            {
                foreach (var user in usersToNotifications)
                {
                    var notification = new Notification()
                    {
                        ProjectId = project.Id,
                        UserId = user.Id,
                        TimeOfNotification = DateTime.Now,
                        TypeOfNotificationId = 4,
                        Content = $"Project Manager {projectManager.Email} został wybrany",
                        Link = link,
                        ToDo = @$"Project manager proszony jest o <br /> 
                            - przygotowanie informacji niezbędnych do TKW <br />
                            - wypełnienie zakładki informacje ekonomiczne <br />
                            - powiadomienie o przygotowanych danych działu finansowego <br />
                            - wypełnienie pozostałych zakładek: <br />
                            - ocena ryzyka, zespół projektowy, interesariusze  <br />",
                        Sent = false
                    };
                    _unitOfWork.NotificationRepository.AddNotification(notification);
                }
            }


            _unitOfWork.Complete();

        }

        private void GenerateNotificationsFinancialDataIsReady(int projectId, string userId)
        {
            var project = _unitOfWork.Project.GetProject(projectId, userId);

            var allUsers = _unitOfWork.UserRepository
                .GetUsers(new UsersFilter(), new PagingInfo())
                .ToList();

            var currentUser = allUsers
                .Single(x => x.Id == project.UserId);


            var projectManager = allUsers
            .SingleOrDefault(x => x.Id == project.ProjectManagerId);

            var usersToNotifications = allUsers
                .Where(x => x.Id == project.ProjectManagerId
                || x.CanConfirmCalculations)
                .ToList();

            // było w warunku  || x.CanConfirmEconomicRequirements


            string link = $@"http://192.168.1.186/mwproject/Project/Project/{project.Id}";

            if (usersToNotifications.Any())
            {
                foreach (var user in usersToNotifications)
                {
                    var notification = new Notification()
                    {
                        ProjectId = project.Id,
                        UserId = user.Id,
                        TimeOfNotification = DateTime.Now,
                        TypeOfNotificationId = 5,
                        Content = $"Dane dla działu finansowego są przygotowane",
                        Link = link,
                        ToDo = @$"Dział Finansowy jest proszony o <br /> 
                            - zweryfikowanie, uzupełnienie i potwierdzenie zakładki dane ekonomiczne <br />
                            - zweryfikowanie, uzupełnienie i potwierdzenie zakładki kalkulacja TKW <br />
                            ",
                        Sent = false
                    };
                    _unitOfWork.NotificationRepository.AddNotification(notification);
                }
            }


            _unitOfWork.Complete();

        }


        // GenerateNotificationsTkwAndEconomicDataAreConfirmed

        private void GenerateNotificationsTkwAndEconomicDataAreConfirmed(int projectId, string userId)
        {

            // Powiadomienie o potwierdzonym TKW dostaje
            // PM
            // Osoba mająca ustawienie: ConfirmedCalculationNotification


            var project = _unitOfWork.Project.GetProject(projectId, userId);

            var allUsers = _unitOfWork.UserRepository
                .GetUsers(new UsersFilter(), new PagingInfo())
                .ToList();

            var currentUser = allUsers
                .Single(x => x.Id == project.UserId);


            var projectManager = allUsers
            .SingleOrDefault(x => x.Id == project.ProjectManagerId);

            var usersToNotifications = allUsers
                .Where(x => x.Id == project.ProjectManagerId
                || x.ConfirmedCalculationNotification
                ).ToList();



            string link = $@"http://192.168.1.186/mwproject/Project/Project/{project.Id}";

            if (usersToNotifications.Any())
            {
                foreach (var user in usersToNotifications)
                {
                    var notification = new Notification()
                    {
                        ProjectId = project.Id,
                        UserId = user.Id,
                        TimeOfNotification = DateTime.Now,
                        TypeOfNotificationId = 6,
                        Content = $"Dane ekonomiczne i kalkulacja TKW została potwierdzona",
                        Link = link,
                        ToDo = @$"Project manager proszony jest o <br /> 
                            - powtórne zweryfikowanie całości Projektu <br />
                            - potwierdzenie Projektu <br />
                            ",
                        Sent = false
                    };
                    _unitOfWork.NotificationRepository.AddNotification(notification);
                }
            }


            _unitOfWork.Complete();

        }


        private void GenerateNotificationsProjectIsConfirmed(int projectId, string userId)
        {
            var project = _unitOfWork.Project.GetProject(projectId, userId);

            var allUsers = _unitOfWork.UserRepository
                .GetUsers(new UsersFilter(), new PagingInfo())
                .ToList();

            var currentUser = allUsers
                .Single(x => x.Id == project.UserId);


            var projectManager = allUsers
            .SingleOrDefault(x => x.Id == project.UserId);

            var usersToNotifications = allUsers
                .Where(x => x.CanAcceptProject).ToList();



            string link = $@"http://192.168.1.186/mwproject/Project/Project/{project.Id}";

            if (usersToNotifications.Any())
            {
                foreach (var user in usersToNotifications)
                {
                    var notification = new Notification()
                    {
                        ProjectId = project.Id,
                        UserId = user.Id,
                        TimeOfNotification = DateTime.Now,
                        TypeOfNotificationId = 7,
                        Content = $"Projekt został zweryfikowany i potwierdzony przez PM",
                        Link = link,
                        ToDo = @$"Sponsor Projektu proszony jest o <br /> 
                            - Zaakceptowanie lub Odrzucenie Projektu <br />
                            ",
                        Sent = false
                    };
                    _unitOfWork.NotificationRepository.AddNotification(notification);
                }
            }


            _unitOfWork.Complete();

        }


        // GenerateNotificationsProjectIsAccepted

        private void GenerateNotificationsProjectIsAccepted(int projectId, string userId)
        {
            var project = _unitOfWork.Project.GetProject(projectId, userId);

            var allUsers = _unitOfWork.UserRepository
                .GetUsers(new UsersFilter(), new PagingInfo())
                .ToList();

            var currentUser = allUsers
                .Single(x => x.Id == project.UserId);


            var projectManager = allUsers
            .SingleOrDefault(x => x.Id == project.ProjectManagerId);

            var usersToNotifications = allUsers
                .Where(x => x.Id == project.ProjectManagerId).ToList();



            string link = $@"http://192.168.1.186/mwproject/Project/Project/{project.Id}";

            if (usersToNotifications.Any())
            {
                foreach (var user in usersToNotifications)
                {
                    var notification = new Notification()
                    {
                        ProjectId = project.Id,
                        UserId = user.Id,
                        TimeOfNotification = DateTime.Now,
                        TypeOfNotificationId = 8,
                        Content = $"Projekt został zaakceptowany",
                        Link = link,
                        ToDo = @$"Project manager proszony jest o <br /> 
                            - start realizacji projektu <br />
                            ",
                        Sent = false
                    };
                    _unitOfWork.NotificationRepository.AddNotification(notification);
                }
            }


            _unitOfWork.Complete();

        }

        #endregion

        #region potwierdzanie Wniosku Projektowego
        public void ConfirmRequest(int id, string userId)
        {
            _unitOfWork.Project.ConfirmRequest(id, userId);
            _unitOfWork.Complete();
            GenerateNotificationsRequestIsConfirmed(id, userId);
            SendNotifications(id, 3, userId);
        }

        public void WithdrawRequestConfimration(int id, string userId)
        {
            _unitOfWork.Project.WithdrawRequestConfimration(id, userId);
            _unitOfWork.Complete();
        }
        #endregion

        #region potwierdzenie Projektu przez PM
        // potwierdzony projekt będzie widoczny jako gotowy do akceptacji przez Szefa
        public void ConfirmProject(int id, string userId)
        {

            _unitOfWork.Project.ConfirmProject(id, userId);
            _unitOfWork.Complete();
            GenerateNotificationsProjectIsConfirmed(id, userId);
            _unitOfWork.Complete();
            SendNotifications(id, 7, userId);
        }

        public void WithdrawProjectConfimration(int id, string userId)
        {
            _unitOfWork.Project.WithdrawProjectConfimration(id, userId);
            _unitOfWork.Complete();
        }
        #endregion

        #region akceptacja Projektu przez Szefa
        public void AcceptProject(int id, string userId)
        {
            _unitOfWork.Project.AcceptProject(id, userId);
            _unitOfWork.Complete();
            GenerateNotificationsProjectIsAccepted(id, userId);
            _unitOfWork.Complete();
            SendNotifications(id, 8, userId);
        }

        public void WithdrawProjectAcceptance(int id, string userId)
        {
            _unitOfWork.Project.WithdrawProjectAcceptance(id, userId);
            _unitOfWork.Complete();
        }
        #endregion

        #region potwierdzenie informacji ekonomicznych
        public void ConfirmEconomicRequirements(int id, string userId)
        {
            _unitOfWork.Project.ConfirmEconomicRequirements(id, userId);
            _unitOfWork.Complete();
            if (CheckIfTkwAndEconomicDataAreConfirmed(id, userId))
            {
                GenerateNotificationsTkwAndEconomicDataAreConfirmed(id, userId);
                SendNotifications(id, 6, userId);
            }
        }

        public void WithdrawConfirmationOfEconomicRequirements(int id, string userId)
        {
            _unitOfWork.Project.WithdrawConfirmationOfEconomicRequirements(id, userId);
            _unitOfWork.Complete();
        }
        #endregion

        #region potwierdzanie kalkulacji TKW
        public void ConfirmCalculation(int id, string userId)
        {
            _unitOfWork.Project.ConfirmCalculation(id, userId);
            _unitOfWork.Complete();
            if (CheckIfTkwAndEconomicDataAreConfirmed(id, userId))
            {
                GenerateNotificationsTkwAndEconomicDataAreConfirmed(id, userId);
                SendNotifications(id, 6, userId);
            }
        }

        public void WithdrawConfirmationOfCalculation(int id, string userId)
        {
            _unitOfWork.Project.WithdrawConfirmationOfCalculation(id, userId);
            _unitOfWork.Complete();
        }

        #endregion

        #region potwierdzanie zespołu projektowego
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

        #endregion 

        #region wczytanie domyślnych informacji do zakładek informacyjnych

        public void AddGeneralRequirementsToProject(Project project)
        {
            _unitOfWork.Project.AddGeneralRequirementsToProject(project);
            _unitOfWork.Complete();
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
        #endregion

        #region potwierdzanie informacji podstawowych
        // sprawdzamy czy są potwierdzone wszystkie zakładki
        // jeżeli tak to wysyłamy maila z powiadomieniem
        // i aktywujemy funkcję potwierdzenia całego wniosku
        public void ConfirmTechnicalProperties(int id, string userId)
        {
            _unitOfWork.Project.ConfirmTechnicalProperties(id, userId);
            _unitOfWork.Complete();
            if (CheckIfAllTabsAreConfirmed(id, userId))
            {
                GenerateNotificationsAllTabsAreConfirmed(id, userId);
                SendNotifications(id, 2, userId);
            }
        }

        public void WithdrawConfirmationOfTechnicalProperties(int id, string userId)
        {
            _unitOfWork.Project.WithdrawConfirmationOfTechnicalProperties(id, userId);
            _unitOfWork.Complete();
        }

        public void ConfirmEstimatedSales(int id, string userId)
        {
            _unitOfWork.Project.ConfirmEstimatedSales(id, userId);
            _unitOfWork.Complete();
            if (CheckIfAllTabsAreConfirmed(id, userId))
            {
                GenerateNotificationsAllTabsAreConfirmed(id, userId);
                SendNotifications(id, 2, userId);
            }
        }
        public void WithdrawConfirmationOfEstimatedSales(int id, string userId)
        {
            _unitOfWork.Project.WithdrawConfirmationOfEstimatedSales(id, userId);
            _unitOfWork.Complete();
        }

        public void ConfirmGeneralRequirements(int id, string userId)
        {
            _unitOfWork.Project.ConfirmGeneralRequirements(id, userId);
            _unitOfWork.Complete();
            if (CheckIfAllTabsAreConfirmed(id, userId))
            {
                GenerateNotificationsAllTabsAreConfirmed(id, userId);
                SendNotifications(id, 2, userId);
            }
        }

        public void WithdrawConfirmationOfGeneralRequirements(int id, string userId)
        {
            _unitOfWork.Project.WithdrawConfirmationOfGeneralRequirements(id, userId);
            _unitOfWork.Complete();
        }


        public void ConfirmQualityRequirements(int id, string userId)
        {
            _unitOfWork.Project.ConfirmQualityRequirements(id, userId);
            _unitOfWork.Complete();
            if (CheckIfAllTabsAreConfirmed(id, userId))
            {
                GenerateNotificationsAllTabsAreConfirmed(id, userId);
                SendNotifications(id, 2, userId);
            }
        }

        public void WithdrawConfirmationOfQualityRequirements(int id, string userId)
        {
            _unitOfWork.Project.WithdrawConfirmationOfQualityRequirements(id, userId);
            _unitOfWork.Complete();
        }

        private bool CheckIfAllTabsAreConfirmed(int id, string userId)
        {
            var project = _unitOfWork.Project.GetProject(id, userId);

            return project.IsTechnicalProportiesConfirmed
            && project.IsGeneralRequirementsConfirmed
            && project.IsEstimatedSalesConfirmed
            && project.IsQualityRequirementsConfirmed;
        }

        #endregion

        #region priorytet projektu

        public void CalculatePriorityOfProject(int id, string userId)
        {
            var selectedProject = _unitOfWork.Project.GetProject(id, userId);
            var projectRequirements = _unitOfWork.Project
                .GetProjectRequirements(id, userId);

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

            decimal estimatedCostOfProject = projectRequirements
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


        public void CalculateTkwInAllProjects(string userId)
        {
            var projects = _unitOfWork.Project.GetAllProjects(userId);
            foreach (var project in projects)
            {
                var selectedProject = _unitOfWork.Project.GetProject(project.Id, userId);

                foreach (var calculation in selectedProject.Calculations)
                {
                    _unitOfWork.Calculation.UpdateCalculation(calculation, userId);
                }

            }
        }

        #endregion

        #region generowanie treści HTML z powiadomieniem

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


            html +=
                $@"<tr>
                    <td align=center bgcolor=lightgrey>Link</td>                    
                    <td align=center bgcolor=white>
                    {notification?.Link}
                    </td>
                </tr>
                ";


            if (!string.IsNullOrEmpty(notification.Content))
            {


                html +=
                    $@"<tr>
                    <td align=center bgcolor=lightgrey>Treść</td>                    
                    <td align=center bgcolor=white>
                    {notification?.Content}
                    </td>
                </tr>
                ";
            }

            if (!string.IsNullOrEmpty(notification.ToDo))
            {


                html +=
                    $@"<tr>
                    <td align=center bgcolor=lightgrey>Do zrobienia</td>                    
                    <td align=center bgcolor=white>
                    {notification?.ToDo}
                    </td>
                </tr>
                ";
            }

            html += $@"</table> <br /> <br /> <i>Automatyczna wiadomość wysłana z aplikacji MWProject</i>";

            return html;
        }
        #endregion

        #region wysyłanie do Excela
        public string ExportProjectsToExcel(IEnumerable<Project> projects, string userId)
        {
            // PM> Install-Package ClosedXML

            // UWAGA ISS Musi mieć uprawnienia do zapisu na serwerze

            string fileName = "ListOfProjects.xlsx";
            // string outputDirectory = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Generated");
            //  string fileNameWithFullPath = Path.Combine(outputDirectory, "ExcelFiles",fileName);

            string directory = @".\ExcelFiles";

            string fileNameWithFullPath = $@"{directory}\{fileName}";



            using (var workbook = new XLWorkbook())
            {

                ExcelAddMainSheet(projects, workbook, userId);
                ExcelAddProjectCardSheet(projects, workbook, userId);
                ExcelAddRequirementsSheet(projects, workbook, userId, (int)RequirementType.Quality, "QualityReq");
                ExcelAddRequirementsSheet(projects, workbook, userId, (int)RequirementType.General, "GeneralReq");
                ExcelAddRequirementsSheet(projects, workbook, userId, (int)RequirementType.Economic, "EconomicReq");
                ExcelAddPropertiesSheet(projects, workbook, userId);
                ExcelAddEstimatedSalesValuesSheet(projects, workbook, userId,"EstimatedSales");
                ExcelAddCalculationsValuesSheet(projects, workbook, userId,"TKW");
                ExcelAddRisksSheet(projects, workbook, userId,"Risks");
                ExcelAddTeamMembersSheet(projects, workbook, userId,"TeamMembers");
                ExcelAddClientsSheet(projects, workbook, userId,"Interesariusze");


                // worksheet.Cell($"A{row}").FormulaA1 = "=MID(A1, 7, 5)";
                workbook.SaveAs(fileNameWithFullPath);
            }
            return fileNameWithFullPath;
        }

        private void ExcelAddRequirementsSheet(IEnumerable<Project> projects, XLWorkbook workbook, string userId, int typeOfRequirement, string nameOfSheet)
        {
            var worksheet = workbook.Worksheets.Add(nameOfSheet);
            int row = 1;
            worksheet.Cell(row, "A").Value = "Id";
            worksheet.Cell(row, "B").Value = "Numer";
            worksheet.Cell(row, "C").Value = "Wymaganie";
            worksheet.Cell(row, "D").Value = "Wartość";
            worksheet.Cell(row, "E").Value = "Komentarz";
            worksheet.Cell(row, "F").Value = "Tak/Nie";
            row++;

            foreach (var project in projects)
            {
                var x = projects.SingleOrDefault(x => x.Id == project.Id);
                var requirements = GetProjectRequirements(project.Id, userId)
                    .Where(x => x.Requirement.Type == typeOfRequirement)
                    .OrderBy(x=>x.Requirement?.Name);

                if (requirements != null)
                {
                    foreach (var item in requirements)
                    {

                        string exist;
                        switch (item.YesNo)
                        {
                            case 1:
                                exist = "TAK";
                                break;
                            case 2:
                                exist = "NIE";
                                break;
                            default:
                                exist = "";
                                break;
                        }


                        worksheet.Cell(row, "A").Value = project.Id;
                        worksheet.Cell(row, "B").RichText.AddText(project.Number).SetBold();
                        worksheet.Cell(row, "C").Value = item.Requirement?.Name;
                        worksheet.Cell(row, "D").Value = item.Value;
                        worksheet.Cell(row, "E").Value = item.Comment?.Trim()?.TrimStart('\r', '\n');
                        worksheet.Cell(row, "F").Value = exist;

                        if (row % 2 == 0)
                        {
                            worksheet.Row(row).Style.Fill.SetBackgroundColor(XLColor.Khaki);
                        }
                        row++;
                    }
                    
                    
                }
               
            }

            worksheet.SheetView.FreezeRows(1);
            worksheet.AutoFilter.Clear();
            worksheet.RangeUsed().SetAutoFilter();

            for (int i = 1; i < 10; i++)
            {
                worksheet.Column(i).AdjustToContents();
                worksheet.Column(i).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

            }
        }


        // var estimatedSalesValues = Model.Project.EstimatedSalesValues;

        private void ExcelAddEstimatedSalesValuesSheet(IEnumerable<Project> projects, XLWorkbook workbook, string userId, string nameOfSheet)
        {
            var worksheet = workbook.Worksheets.Add(nameOfSheet);
            int row = 1;
            worksheet.Cell(row, "A").Value = "Id";
            worksheet.Cell(row, "B").Value = "Numer";
            worksheet.Cell(row, "C").Value = "Rok";
            worksheet.Cell(row, "D").Value = "Cena";
            worksheet.Cell(row, "E").Value = "Ilość";
            worksheet.Cell(row, "F").Value = "Wartość";
            row++;

            foreach (var project in projects)
            {
                var fullProject = GetProject(project.Id, userId);
                var estimatedSalesValues = fullProject.EstimatedSalesValues;
                

                if (estimatedSalesValues != null)
                {
                    foreach (var item in estimatedSalesValues)
                    {

                        worksheet.Cell(row, "A").Value = project.Id;
                        worksheet.Cell(row, "B").RichText.AddText(project.Number).SetBold();
                        worksheet.Cell(row, "C").Value = item.Year;
                        worksheet.Cell(row, "D").Value = item.Price;
                        worksheet.Cell(row, "E").Value = item.Qty;
                        worksheet.Cell(row, "F").Value = Math.Round(item.Qty * item.Price, 2);

                        if (row % 2 == 0)
                        {
                            worksheet.Row(row).Style.Fill.SetBackgroundColor(XLColor.Khaki);
                        }
                        row++;
                    }


                }

            }

            worksheet.SheetView.FreezeRows(1);
            worksheet.AutoFilter.Clear();
            worksheet.RangeUsed().SetAutoFilter();

            for (int i = 1; i < 10; i++)
            {
                worksheet.Column(i).AdjustToContents();
                worksheet.Column(i).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

            }
        }


        private void ExcelAddRisksSheet(IEnumerable<Project> projects, XLWorkbook workbook, string userId, string nameOfSheet)
        {
            var worksheet = workbook.Worksheets.Add(nameOfSheet);
            int row = 1;
            worksheet.Cell(row, "A").Value = "Id";
            worksheet.Cell(row, "B").Value = "Numer";
            worksheet.Cell(row, "C").Value = "Nazwa";
            worksheet.Cell(row, "D").Value = "Komentarz";
            row++;

            foreach (var project in projects)
            {
                var fullProject = GetProject(project.Id, userId);
                var risks = fullProject.ProjectRisks;


                if (risks != null)
                {
                    foreach (var item in risks)
                    {

                        worksheet.Cell(row, "A").Value = project.Id;
                        worksheet.Cell(row, "B").RichText.AddText(project.Number).SetBold();
                        worksheet.Cell(row, "C").Value = item.Name;
                        worksheet.Cell(row, "D").Value = item.Content?.Trim()?.TrimStart('\r', '\n');

                        if (row % 2 == 0)
                        {
                            worksheet.Row(row).Style.Fill.SetBackgroundColor(XLColor.Khaki);
                        }
                        row++;
                    }


                }

            }

            worksheet.SheetView.FreezeRows(1);
            worksheet.AutoFilter.Clear();
            worksheet.RangeUsed().SetAutoFilter();

            for (int i = 1; i < 10; i++)
            {
                worksheet.Column(i).AdjustToContents();
                worksheet.Column(i).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

            }
        }

        private void ExcelAddClientsSheet(IEnumerable<Project> projects, XLWorkbook workbook, string userId, string nameOfSheet)
        {
            var worksheet = workbook.Worksheets.Add(nameOfSheet);
            int row = 1;
            worksheet.Cell(row, "A").Value = "Id";
            worksheet.Cell(row, "B").Value = "Numer";
            worksheet.Cell(row, "C").Value = "Nazwa";
            row++;

            foreach (var project in projects)
            {
                var fullProject = GetProject(project.Id, userId);
                var clients = fullProject.ProjectClients;


                if (clients != null)
                {
                    foreach (var item in clients)
                    {

                        worksheet.Cell(row, "A").Value = project.Id;
                        worksheet.Cell(row, "B").RichText.AddText(project.Number).SetBold();
                        worksheet.Cell(row, "C").Value = item.Name;

                        if (row % 2 == 0)
                        {
                            worksheet.Row(row).Style.Fill.SetBackgroundColor(XLColor.Khaki);
                        }
                        row++;
                    }


                }

            }

            worksheet.SheetView.FreezeRows(1);
            worksheet.AutoFilter.Clear();
            worksheet.RangeUsed().SetAutoFilter();

            for (int i = 1; i < 10; i++)
            {
                worksheet.Column(i).AdjustToContents();
                worksheet.Column(i).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

            }
        }

        private void ExcelAddTeamMembersSheet(IEnumerable<Project> projects, XLWorkbook workbook, string userId, string nameOfSheet)
        {
            var worksheet = workbook.Worksheets.Add(nameOfSheet);
            int row = 1;
            worksheet.Cell(row, "A").Value = "Id";
            worksheet.Cell(row, "B").Value = "Numer";
            worksheet.Cell(row, "C").Value = "Imię i Nazwisko";
            worksheet.Cell(row, "D").Value = "Komentarz";
            row++;

            foreach (var project in projects)
            {
                var fullProject = GetProject(project.Id, userId);
                var teamMembers = fullProject.ProjectTeamMembers;


                if (teamMembers != null)
                {
                    foreach (var item in teamMembers)
                    {

                        worksheet.Cell(row, "A").Value = project.Id;
                        worksheet.Cell(row, "B").RichText.AddText(project.Number).SetBold();
                        worksheet.Cell(row, "C").Value = item.User?.FirstName?.Trim() + item.User?.LastName?.Trim() ;
                        worksheet.Cell(row, "D").Value = item.Description?.Trim()?.TrimStart('\r', '\n');

                        if (row % 2 == 0)
                        {
                            worksheet.Row(row).Style.Fill.SetBackgroundColor(XLColor.Khaki);
                        }
                        row++;
                    }


                }

            }

            worksheet.SheetView.FreezeRows(1);
            worksheet.AutoFilter.Clear();
            worksheet.RangeUsed().SetAutoFilter();

            for (int i = 1; i < 10; i++)
            {
                worksheet.Column(i).AdjustToContents();
                worksheet.Column(i).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

            }
        }

        private void ExcelAddCalculationsValuesSheet(IEnumerable<Project> projects, XLWorkbook workbook, string userId, string nameOfSheet)
        {
            var worksheet = workbook.Worksheets.Add(nameOfSheet);
            int row = 1;
            worksheet.Cell(row, "A").Value = "Id";
            worksheet.Cell(row, "B").Value = "Numer";
            worksheet.Cell(row, "C").Value = "Tytuł";
            worksheet.Cell(row, "D").Value = "Robocizna";
            worksheet.Cell(row, "E").Value = "Materiał";
            worksheet.Cell(row, "F").Value = "Narzut";
            worksheet.Cell(row, "G").Value = "Tkw";
            worksheet.Cell(row, "H").Value = "Ckw";
            worksheet.Cell(row, "I").Value = "Komentarz";
            row++;

            foreach (var project in projects)
            {
                var fullProject = GetProject(project.Id, userId);
                var calculations = fullProject.Calculations;


                if (calculations != null)
                {
                    foreach (var item in calculations)
                    {

                        worksheet.Cell(row, "A").Value = project.Id;
                        worksheet.Cell(row, "B").RichText.AddText(project.Number).SetBold();
                        worksheet.Cell(row, "C").Value = item.Title;
                        worksheet.Cell(row, "D").Value = item.MaterialCosts;
                        worksheet.Cell(row, "E").Value = item.LabourCosts;
                        worksheet.Cell(row, "F").Value = item.Markup;
                        worksheet.Cell(row, "G").Value = item.Tkw;
                        worksheet.Cell(row, "H").Value = item.Ckw;
                        worksheet.Cell(row, "I").Value = item.Comment?.Trim()?.TrimStart('\r', '\n');


                        if (row % 2 == 0)
                        {
                            worksheet.Row(row).Style.Fill.SetBackgroundColor(XLColor.Khaki);
                        }
                        row++;
                    }


                }

            }

            worksheet.SheetView.FreezeRows(1);
            worksheet.AutoFilter.Clear();
            worksheet.RangeUsed().SetAutoFilter();

            for (int i = 1; i < 10; i++)
            {
                worksheet.Column(i).AdjustToContents();
                worksheet.Column(i).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

            }
        }

        private void ExcelAddPropertiesSheet(IEnumerable<Project> projects, XLWorkbook workbook, string userId)
        {
            var worksheet = workbook.Worksheets.Add("Properties");
            int row = 1;
            worksheet.Cell(row, "A").Value = "Id";
            worksheet.Cell(row, "B").Value = "Numer";
            worksheet.Cell(row, "C").Value = "Wymaganie";
            worksheet.Cell(row, "D").Value = "Wartość";
            worksheet.Cell(row, "E").Value = "Komentarz";
            worksheet.Cell(row, "F").Value = "Tak/Nie";
            row++;

            foreach (var project in projects)
            {
                var x = projects.SingleOrDefault(x => x.Id == project.Id);
                var requirements = GetTechnicalProperties(project.Id, userId);

                if (requirements != null)
                {
                    foreach (var item in requirements)
                    {

                        string exist;
                        switch (item.YesNo)
                        {
                            case 1:
                                exist = "TAK";
                                break;
                            case 2:
                                exist = "NIE";
                                break;
                            default:
                                exist = "";
                                break;
                        }

                        worksheet.Cell(row, "A").Value = project.Id;
                        worksheet.Cell(row, "B").RichText.AddText(project.Number).SetBold();
                        worksheet.Cell(row, "C").Value = item.TechnicalProperty?.Name;
                        worksheet.Cell(row, "D").Value = item.Value;
                        worksheet.Cell(row, "E").Value = item.Comment?.Trim()?.TrimStart('\r', '\n');
                        worksheet.Cell(row, "F").Value = exist;

                        if (row % 2 == 0)
                        {
                            worksheet.Row(row).Style.Fill.SetBackgroundColor(XLColor.Khaki);
                        }
                        row++;
                    }
                    
                    
                }

            }

            worksheet.SheetView.FreezeRows(1);
            worksheet.AutoFilter.Clear();
            worksheet.RangeUsed().SetAutoFilter();

            for (int i = 1; i < 6; i++)
            {
                worksheet.Column(i).AdjustToContents();
                worksheet.Column(i).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

            }
        }
        private void ExcelAddMainSheet(IEnumerable<Project> projects, XLWorkbook workbook, string userId)
        {
            var worksheet = workbook.Worksheets.Add("Projects");

            int row = 1;
            worksheet.Cell(row, "A").Value = "Id";
            worksheet.Cell(row, "B").Value = "Prio";
            worksheet.Cell(row, "C").Value = "Numer";
            worksheet.Cell(row, "D").Value = "Tytuł";
            worksheet.Cell(row, "E").Value = "Opis";
            worksheet.Cell(row, "F").Value = "Kategoria";
            worksheet.Cell(row, "G").Value = "Data_Utworzenia";
            worksheet.Cell(row, "H").Value = "Zakończony";
            worksheet.Cell(row, "I").Value = "Data_Zakonczenia";
            worksheet.Cell(row, "J").Value = "Anulowany";
            worksheet.Cell(row, "K").Value = "Data_Anulowania";
            worksheet.Cell(row, "L").Value = "Zainicjowany_przez";
            worksheet.Cell(row, "M").Value = "PM";
            worksheet.Cell(row, "N").Value = "Potwierdzony";
            worksheet.Cell(row, "O").Value = "Potwierdzony_Przez";
            worksheet.Cell(row, "P").Value = "Data_Potwierdzenia";

            worksheet.Cell(row, "R").Value = "Zaakceptowany";
            worksheet.Cell(row, "S").Value = "Zaakceptowany_Przez";
            worksheet.Cell(row, "T").Value = "Data_Akceptacji";


            worksheet.Cell(row, "U").Value = "Planow_Data_Rozp";
            worksheet.Cell(row, "V").Value = "Fakt_Data_Rozp";
            worksheet.Cell(row, "W").Value = "Planow_Data_Zak";

            worksheet.Cell(row, "X").Value = "Klient";
            worksheet.Cell(row, "Y").Value = "nowy_mod";

            worksheet.Row(row).Style.Fill.SetBackgroundColor(XLColor.Firebrick);
            worksheet.Row(row).Style.Font.SetBold();
            worksheet.Row(row).Style.Font.FontColor = XLColor.White;

            row++;
            foreach (var project in projects)
            {
                worksheet.Cell($"A{row}").Value = project.Id;
                worksheet.Cell(row, "B").Value = project.PriorityOfProject;
                worksheet.Cell(row, "C").RichText.AddText(project.Number).SetBold();
                worksheet.Cell(row, "D").Value = project.Title?.Trim();
                worksheet.Cell(row, "E").Value = project.Description?.Trim();
                worksheet.Cell(row, "F").Value = project.Category?.Name;
                worksheet.Cell(row, "G").Value = project.CreatedDate;
                worksheet.Cell(row, "H").Value = project.IsExecuted;
                worksheet.Cell(row, "I").Value = project.FinishedDate;
                worksheet.Cell(row, "J").Value = project.IsCanceled;
                worksheet.Cell(row, "K").Value = project.CanceledDate;
                worksheet.Cell(row, "L").Value = project.InitiatedBy;
                worksheet.Cell(row, "M").Value = project.ProjectManagerId
                    == null ? string.Empty : _unitOfWork.UserRepository.GetUser(project.ProjectManagerId)?.UserName;

                worksheet.Cell(row, "N").Value = project.IsConfirmed;
                worksheet.Cell(row, "O").Value = project.ConfirmedBy
                    == null ? string.Empty : _unitOfWork.UserRepository.GetUser(project.ConfirmedBy)?.UserName;
                worksheet.Cell(row, "P").Value = project.ConfirmedDate;


                worksheet.Cell(row, "R").Value = project.IsAccepted;
                worksheet.Cell(row, "S").Value = project.AcceptedBy
                    == null ? string.Empty : _unitOfWork.UserRepository.GetUser(project.AcceptedBy)?.UserName;
                worksheet.Cell(row, "T").Value = project.AcceptedDate;


                worksheet.Cell(row, "U").Value = project.PlannedStartDateOfTheProject;
                worksheet.Cell(row, "V").Value = project.RealStartDateOfTheProject;
                worksheet.Cell(row, "W").Value = project.PlannedEndDateOfTheProject;

                worksheet.Cell(row, "X").Value = project.Client;


                string productStatus;


                switch (project.ProductStatus)
                {
                    case 0:
                        productStatus = "nd";
                        break;
                    case 1:
                        productStatus = "nowy produkt";
                        break;
                    case 2:
                        productStatus = "modyfikacja produktu";
                        break;

                    default:
                        productStatus = string.Empty;
                        break;
                }


                worksheet.Cell(row, "Y").Value = productStatus;


                // PlannedStartDateOfTheProject RealStartDateOfTheProject  PlannedEndDateOfTheProject

                //worksheet.Cell(row, 3).RichText.AddText(project.Title).SetFontColor(XLColor.Blue).SetBold();


                // Add the text parts
                /*
                var cell = worksheet.Cell(row, "D");
                cell.RichText
                  .AddText("Hello").SetFontColor(XLColor.Red)
                  .AddText(" BIG ").SetFontColor(XLColor.Blue).SetBold()
                  .AddText("World").SetFontColor(XLColor.Red);
                */

                if (row % 2 == 0)
                {
                    worksheet.Row(row).Style.Fill.SetBackgroundColor(XLColor.Khaki);
                    // worksheet.Row(row).Style.Border.SetOutsideBorderColor(XLColor.Blue);
                    // worksheet.Row(row).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                }
                worksheet.Row(row).AdjustToContents();

                // .FromArgb(0xEEFFEE)

                row++;
            }




            worksheet.SheetView.FreezeRows(1);
            worksheet.AutoFilter.Clear();
            worksheet.RangeUsed().SetAutoFilter();

            for (int i = 1; i < 30; i++)
            {
                worksheet.Column(i).AdjustToContents();
                worksheet.Column(i).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

            }

        }

        private void ExcelAddProjectCardSheet(IEnumerable<Project> projects, XLWorkbook workbook, string userId)
        {
            var worksheet = workbook.Worksheets.Add("ProjectCards");

            int row = 1;
            worksheet.Cell(row, "A").Value = "Id";
            worksheet.Cell(row, "B").Value = "Numer";
            worksheet.Cell(row, "C").Value = "Cel";
            worksheet.Cell(row, "D").Value = "Działania weryfikacyjne";
            worksheet.Cell(row, "E").Value = "Uwagi i Komentarze";
            worksheet.Cell(row, "F").Value = "Link do Planera";

            worksheet.Row(row).Style.Fill.SetBackgroundColor(XLColor.Firebrick);
            worksheet.Row(row).Style.Font.SetBold();
            worksheet.Row(row).Style.Font.FontColor = XLColor.White;

            row++;
            foreach (var project in projects)
            {
                worksheet.Cell(row,"A").Value = project.Id;
                worksheet.Cell(row,"B").RichText.AddText(project.Number).SetBold();
                worksheet.Cell(row,"C").Value = project.DescriptionOfPurpose?.Trim()?.TrimStart('\r', '\n');
                worksheet.Cell(row,"D").Value = project.VerificationOperations?.Trim()?.TrimStart('\r', '\n');
                worksheet.Cell(row,"E").Value = project.Comment?.Trim()?.TrimStart('\r', '\n');
                worksheet.Cell(row,"F").Value = project.LinkToPlanner;


                if (row % 2 == 0)
                {
                    worksheet.Row(row).Style.Fill.SetBackgroundColor(XLColor.Khaki);

                }
                worksheet.Row(row).AdjustToContents();
                row++;
            }




            worksheet.SheetView.FreezeRows(1);
            worksheet.AutoFilter.Clear();
            worksheet.RangeUsed().SetAutoFilter();

            for (int i = 1; i < 30; i++)
            {
                worksheet.Column(i).AdjustToContents();
                worksheet.Column(i).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

            }

        }
        #endregion


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

        public void UpdateProjectManager(Project project, string userId)
        {
            var projectManagerWasUpdated = _unitOfWork.Project.UpdateProjectManager(project, userId);
            _unitOfWork.Complete();

            if (projectManagerWasUpdated)
            {
                GenerateNotificationsProjectManagerIsSet(project.Id, userId);
                _unitOfWork.Complete();
                SendNotifications(project.Id, 4, userId);
            }


        }

        public void UpdateFinancialComments(Project project, string userId)
        {
            _unitOfWork.Project.UpdateFinancialComments(project, userId);
            _unitOfWork.Complete();

            GenerateNotificationsFinancialDataIsReady(project.Id, userId);
            _unitOfWork.Complete();
            SendNotifications(project.Id, 5, userId);

        }

        private bool CheckIfTkwAndEconomicDataAreConfirmed(int id, string userId)
        {
            var project = _unitOfWork.Project.GetProject(id, userId);

            return project.IsEconomicRequirementsConfirmed
            && project.IsCalculationConfirmed;

        }

        public void UpdateProjectWithAdminRights(Project project, string userId)
        {
            _unitOfWork.Project.UpdateProjectWithAdminRights(project, userId);
            _unitOfWork.Complete();
        }

        public void UpdateProjectExecution(Project project, string userId)
        {
            _unitOfWork.Project.UpdateProjectExecution(project, userId);
            _unitOfWork.Complete();
        }

        public IEnumerable<Notification> GetNotifications(int ProjectId, string userId)
        {
            return _unitOfWork.Project.GetNotifications(ProjectId, userId);
        }

        public IEnumerable<ProjectRequirement> GetProjectRequirements(int ProjectId, string userId)
        {
            return _unitOfWork.Project.GetProjectRequirements(ProjectId, userId);
        }

        public IEnumerable<ProjectTechnicalProperty> GetTechnicalProperties(int ProjectId, string userId)
        {
            return _unitOfWork.Project.GetTechnicalProperties(ProjectId, userId);
        }
    }
}
