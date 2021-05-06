using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class ApplicationUser : IdentityUser
    {

        [Required]
        [Display(Name = "Imię")]
        public string  FirstName { get; set; }

        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }


        [Required]
        [Display(Name = "Stanowisko")]
        public string Possition { get; set; }


        [Display(Name = "może potwierdzać koszty")]
        public bool CanConfirmCalculations { get; set; }

        [Display(Name = "może potwierdzać prognozowaną sprzedaż")]
        public bool CanConfirmEstimatedSales { get; set; }

        [Display(Name = "może potwierdzać dane techniczne")]
        public bool CanConfirmTechnicalProperties { get; set; }

        [Display(Name = "może potwierdzać wymagania jakościowe")]
        public bool CanConfirmQualityRequirements { get; set; }

        [Display(Name = "może potwierdzać wymagania ekonomiczne")]
        public bool CanConfirmEconomicRequirements { get; set; }

        [Display(Name = "może akceptować projekt")]
        public bool CanAcceptProject { get; set; }
        
        [Display(Name = "może potwierdzać projekt")]
        public bool CanConfirmProject { get; set; }

        [Display(Name = "uprawnienia administratora")]
        public bool AdminRights { get; set; }

        [Display(Name = "może edytować dane")]
        public bool CanModifyProject { get; set; }

        [Display(Name = "widzi wszystkie projekty")]
        public bool CanSeeAllProject { get; set; }

        public ApplicationUser()
        {
            Projects = new Collection<Project>();
        }
        public ICollection<Project> Projects;
        
    }
}
