﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Display(Name = "Numer ewidencyjny")]
        public string ReferenceNumber { get; set; }


        [Display(Name = "może potwierdzać koszty")]
        public bool CanConfirmCalculations { get; set; }

        [Display(Name = "może potwierdzać prognozowaną sprzedaż")]
        public bool CanConfirmEstimatedSales { get; set; }

        [Display(Name = "może potwierdzać informacje techniczne")]
        public bool CanConfirmTechnicalProperties { get; set; }

        [Display(Name = "może potwierdzać informacje jakościowe")]
        public bool CanConfirmQualityRequirements { get; set; }

        [Display(Name = "może potwierdzać informacje ekonomiczne")]
        public bool CanConfirmEconomicRequirements { get; set; }

        [Display(Name = "może potwierdzać informacje ogólne")]
        public bool CanConfirmGeneralRequirements { get; set; }

        [Display(Name = "może potwierdzać zespół projektowy")]
        public bool CanConfirmProjectTeam { get; set; }

        [Display(Name = "może akceptować projekt")]
        public bool CanAcceptProject { get; set; }
        
        [Display(Name = "może potwierdzać projekt")]
        public bool CanConfirmProject { get; set; }
        [Display(Name = "może potwierdzać wniosek")]
        public bool CanConfirmRequest { get; set; }

        [Display(Name = "uprawnienia administratora")]
        public bool AdminRights { get; set; }

        [Display(Name = "system admin")]
        public bool SuperAdminRights { get; set; }

        [Display(Name = "może edytować dane")]
        public bool CanModifyProject { get; set; }

        [Display(Name = "widzi wszystkie projekty")]
        public bool CanSeeAllProject { get; set; }

        [Display(Name = "może edytować koszty")]
        public bool CanEditCalculations { get; set; }

        [Display(Name = "może edytować prognozowaną sprzedaż")]
        public bool CanEditEstimatedSales { get; set; }

        [Display(Name = "może edytować informacje techniczne")]
        public bool CanEditTechnicalProperties { get; set; }

        [Display(Name = "może edytować informacje jakościowe")]
        public bool CanEditQualityRequirements { get; set; }

        [Display(Name = "może edytować informacje ekonomiczne")]
        public bool CanEditEconomicRequirements { get; set; }

        [Display(Name = "może edytować informacje ogólne")]
        public bool CanEditGeneralRequirements { get; set; }

        [Display(Name = "może edytować zespół projektowy")]
        public bool CanEditProjectTeam { get; set; }

        [Display(Name = "może ustanowić PM")]
        public bool CanSetProjectManager { get; set; }

        [Display(Name = "manager")]
        public bool IsManager { get; set; }


        [ForeignKey("Manager")]
        public string ManagerId { get; set; }
        public ApplicationUser Manager { get; set; }

        
        [Display(Name = "email gdy nowy projekt")]
        public bool NewProjectEmailNotification { get; set; }

        [Display(Name = "email gdy potwierdzone TKW")]
        public bool ConfirmedCalculationNotification { get; set; }


        
        

        // [InverseProperty("ProjectManager")]
        
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public ICollection<ProjectTeamMember> ProjectTeamMembers { get; set; }  = new HashSet<ProjectTeamMember>();

        public ICollection<Project> ProjectAuthors  = new HashSet<Project>();
        public ICollection<Project> ProjectManagers { get; set; } = new HashSet<Project>();
        
        public ICollection<Project> ProjectTeamConfirmedUsers { get; set; } = new HashSet<Project>();
    }
}
