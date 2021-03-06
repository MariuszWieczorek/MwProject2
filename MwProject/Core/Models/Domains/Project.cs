using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class Project
    {
        public int Id { get; set; }

        [Display(Name = "Lp")]
        public int OrdinalNumber { get; set; }

        [MaxLength(255)]
        [Required(ErrorMessage = "Pole tytuł jest wymagane.")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [MaxLength(50)]
        [Display(Name = "Numer")]
        public string Number { get; set; }

        [Display(Name = "Nr")]
        public int No { get; set; }

        
        
        [Display(Name = "Cel projektu - szczegóły")]
        public string DescriptionOfPurpose { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Uwagi / komentarze")]
        public string Comment { get; set; }

        
        
        // powiadowmienia do działu finansów
        [Display(Name = "Uwagi finansowe")]
        public string FinancialComments { get; set; }

        [Display(Name = "Czas wyboru PM")]
        public DateTime? FinancialNotificationDate { get; set; }

        [Display(Name = "Kto wysłał powiadomienie do finansów")]
        public string FinancialNotificationBy { get; set; }



        [Display(Name = "Klient / Dział")]
        public string Client { get; set; }

        [Display(Name = "działania weryfikacyjne przed uruchomieniem projektu")]
        public string VerificationOperations { get; set; }

        [Display(Name = "link do harmonogramu")]
        public string LinkToPlanner { get; set; }

        [Display(Name = "Data Utworzenia")]
        public DateTime? CreatedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Display(Name = "Oczekiwany Termin")]
        public DateTime? Term { get; set; }

        
        
        [Display(Name = "Data zakończenia")]
        public DateTime? FinishedDate { get; set; }

        [Display(Name = "zrealizowany")]
        public bool IsExecuted { get; set; }


        
        [Display(Name = "Data anulowania")]
        public DateTime? CanceledDate { get; set; }

        [Display(Name = "anulowany")]
        public bool IsCanceled { get; set; }





        // Opis słowny kto zainicjował wniosek projektowy
        [Required(ErrorMessage = "Pole zainicjowane przez jest wymagane.")]
        [Display(Name = "Wniosek zainicjowany przez")]
        public string InitiatedBy { get; set; }



        
        
        // Project Manager

        [Display(Name = "Kierownik Projektu")]
        [ForeignKey("ProjectManager")]
        public string ProjectManagerId { get; set; }
        public ApplicationUser ProjectManager { get; set; }

        [Display(Name = "Czas wyboru PM")]
        public DateTime? ProjectManagerSetDate { get; set; }

        [Display(Name = "Kto Wybrał PM")]
        public string ProjectManagerSetBy { get; set; }




        [Required(ErrorMessage = "Pole kategoria jest wymagane.")]
        [Display(Name = "Kategoria")]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

       
        [Display(Name = "Program")]
        public int? ProjectGroupId { get; set; }
        public ProjectGroup ProjectGroup { get; set; }



        [Display(Name = "Status")]
        public int? ProjectStatusId { get; set; }
        public ProjectStatus ProjectStatus { get; set; }


        [Display(Name = "Nowy Produkt")]
        public bool NewProduct { get; set; }

        [Display(Name = "Nowy Asortyment")]
        public bool NewAssortment { get; set; }


        [Column(TypeName = "tinyint")]
        [Display(Name = "Nowy/Modyfikacja")]
        public int ProductStatus { get; set; }


        [Display(Name = "Grupa Produktu")]
        public int? ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }



        // kalkulacja

        [Display(Name = "Kalkulacja potwierdzona przez")]
        public string CalculationConfirmedBy { get; set; }

        [Display(Name = "Czas potwierdzenia kalkulacji")]
        public DateTime? CalculationConfirmedDate { get; set; }

        [Display(Name = "Czy kalkulacja potwierdzona")]
        public bool IsCalculationConfirmed { get; set; }

        // przewidywana sprzedaż

        [Display(Name = "Przewidywana sprzedaż potwierdzona przez")]
        public string EstimatedSalesConfirmedBy { get; set; }

        [Display(Name = "Czas potwierdzenia przewidywanej sprzedaży")]
        public DateTime? EstimatedSalesConfirmedDate { get; set; }

        [Display(Name = "Czy zatwierdzona przewidywana sprzedaż")]
        public bool IsEstimatedSalesConfirmed { get; set; }

        
        
        // potwierdzenie wniosku

        [Display(Name = "Wniosek potwierdzony przez")]
        public string RequestConfirmedBy { get; set; }

        [Display(Name = "Czas potwierdzenia wniosku")]
        public DateTime? RequestConfirmedDate { get; set; }

        [Display(Name = "Czy Wniosek został potwierdzony")]
        public bool IsRequestConfirmed { get; set; }


        // potwierdzenie projektu

        [Display(Name = "Projekt potwierdzony przez")]
        public string ConfirmedBy { get; set; }

        [Display(Name = "Czas potwierdzenia projektu")]
        public DateTime? ConfirmedDate { get; set; }
        
        [Display(Name = "Czy projekt został potwierdzony")]
        public bool IsConfirmed { get; set; }


        
        
        // akceptacja projektu przez prezesa

        [Display(Name = "Projekt zaakceptowany przez")]
        public string AcceptedBy { get; set; }
        
        [Display(Name = "Czas zaakceptowania projektu")]
        public DateTime? AcceptedDate { get; set; }

        [Display(Name = "Czy projekt został zaakceptowany")]
        public bool IsAccepted { get; set; }


        
        
        
        // potwierdzenie informacji jakościowych

        [Display(Name = "Wymagania jakościowe potwierdzone przez")]
        public string QualityRequirementsConfirmedBy { get; set; }

        [Display(Name = "Czas potwierdzenia wymagań jakościowych")]
        public DateTime? QualityRequirementsConfirmedDate { get; set; }

        [Display(Name = "Czy wymagania jakościowe zostały potwierdzone")]
        public bool IsQualityRequirementsConfirmed { get; set; }


        
        
        // potwierdzenie informacji ekonomicznych

        [Display(Name = "Wymagania ekonomiczne potwierdzone przez")]
        public string EconomicRequirementsConfirmedBy { get; set; }

        [Display(Name = "Czas potwierdzenia wymagań ekonomicznych")]
        public DateTime? EconomicRequirementsConfirmedDate { get; set; }

        [Display(Name = "Czy wymagania ekonomiczne zostały potwierdzone")]
        public bool IsEconomicRequirementsConfirmed { get; set; }

        
        
        // potwierdzenie informacji ogólnych

        [Display(Name = "Informacje ogólne potwierdzone przez")]
        public string GeneralRequirementsConfirmedBy { get; set; }

        [Display(Name = "Czas potwierdzenia informacji ogólnych")]
        public DateTime? GeneralRequirementsConfirmedDate { get; set; }

        [Display(Name = "Czy informacje ogólne zostały potwierdzone")]
        public bool IsGeneralRequirementsConfirmed { get; set; }

        
        
        // potwierdzenie informacji technicznych

        [Display(Name = "Cechy techniczne potwierdzone przez")]
        public string TechnicalProportiesConfirmedBy { get; set; }

        [Display(Name = "Czas potwierdzenia cech technicznych")]
        public DateTime? TechnicalProportiesConfirmedDate { get; set; }

        [Display(Name = "Czy cechy techniczne zostały potwierdzone")]
        public bool IsTechnicalProportiesConfirmed { get; set; }


        
        
        // potwierdzenie zespołu projektowego

        [Display(Name = "Zespół projektowy potwierdzone przez")]
        public string ProjectTeamConfirmedBy { get; set; }

        [Display(Name = "Czas potwierdzenia zespołu projektowego")]
        public DateTime? ProjectTeamConfirmedDate { get; set; }

        [Display(Name = "Czy zespół projektowy został potwierdzony")]
        public bool IsProjectTeamConfirmed { get; set; }

          
        
        
        
        [Display(Name = "Autor")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        
        
        [Display(Name = "Konkurencyjność")]
        [ForeignKey("CompetitivenessOfTheProject")]
        public int? CompetitivenessOfTheProjectId { get; set; }
        public RankingElement CompetitivenessOfTheProject { get; set; }

        
        
        [Display(Name = "Cel projektu")]
        [ForeignKey("PurposeOfTheProject")]
        public int? PurposeOfTheProjectId { get; set; }
        public RankingElement PurposeOfTheProject { get; set; }

       
        
        [Display(Name = "Wykonalność")]
        [ForeignKey("ViabilityOfTheProject")]
        public int? ViabilityOfTheProjectId { get; set; }
        public RankingElement ViabilityOfTheProject { get; set; }

        [Display(Name = "Priorytet")]
        public int PriorityOfProject { get; set; }

        
        [Display(Name = "Ranking SCZ")]
        public int RankingOfEstimatedPaybackTimeInMonths { get; set; }

        [Display(Name = "Czas zwrotu w mc")]
        public int EstimatedPaybackTimeInMonths { get; set; }


        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Szacowany Koszt Projektu")]
        public decimal EstimatedCostOfProject { get; set; }


        [Display(Name = "Fakt. data rozpoczęcia")]
        public DateTime? RealStartDateOfTheProject { get; set; }

        [Display(Name = "Planow. data rozpoczęcia")]
        public DateTime? PlannedStartDateOfTheProject { get; set; }

        [Display(Name = "Planow. data zakończenia")]
        public DateTime? PlannedEndDateOfTheProject { get; set; }

        [Display(Name = "Czas wdrożenia w mc")]
        public int ImplementationTimeInMonths { get; set; }

        [Display(Name = "Rank CDS")]
        public int RankingOfImplementationTimeInMonths { get; set; }


        [Display(Name = "ROI")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ReturnOnInvestment { get; set; }

        [Display(Name = "Rank ROI")]
        public int RankingOfReturnOnInvestment { get; set; }


        [Display(Name = "Zdolność produkcji przed realizacją projektu")]
        public int ProductionCapacity { get; set; }

        [Display(Name = "Zdolność produkcji po realizacji projektu")]
        [Column(TypeName = "decimal(18, 2)")]
        public int PlannedProductionVolume { get; set; }
        
        [Display(Name = "% WZP")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PercentageOfUsedProductionCapability { get; set; }

        [Display(Name = "Rank WZP")]
        public int RankingOfUsedProductionCapability { get; set; }
        
        
        // Powiązane Listy
        public ICollection<Calculation> Calculations { get; set; } = new HashSet<Calculation>();   
        public ICollection<EstimatedSalesValue> EstimatedSalesValues { get; set; } = new Collection<EstimatedSalesValue>();
        public ICollection<ProjectRequirement> ProjectRequirements { get; set; } = new HashSet<ProjectRequirement>();   
        public ICollection<ProjectTechnicalProperty> ProjectTechnicalProperties { get; set; } = new HashSet<ProjectTechnicalProperty>();
        public ICollection<ProjectTeamMember> ProjectTeamMembers { get; set; }  = new Collection<ProjectTeamMember>();
        public ICollection<Notification> Notifications { get; set; } = new Collection<Notification>();
        public ICollection<ProjectClient> ProjectClients { get; set; } = new Collection<ProjectClient>();  
        public ICollection<ProjectRisk> ProjectRisks { get; set; } = new Collection<ProjectRisk>();


        // nie używane, do usunięcia
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Wartość")]
        public decimal Value { get; set; }

        [MaxLength(250)]
        [Display(Name = "Obrazek")]
        public string Picture { get; set; }

        [Display(Name = "Koordynator")]
        public string Coordinator { get; set; }

    }
}

/* https://editor.ponyorm.com/user/mariusz/Project/designer */

