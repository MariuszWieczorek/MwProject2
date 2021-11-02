using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Filters
{
    public class ProjectsFilter
    {
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Display(Name = "Klient")]
        public string Client { get; set; }

        [Display(Name = "Kategoria")]
        public int? CategoryId { get; set; }
        
        [Display(Name = "Program")]
        public int? ProjectGroupId { get; set; }

        [Display(Name = "Status")]
        public int? ProjectStatusId { get; set; }

        [Display(Name = "Numer")]
        public string Number { get; set; }

        
        [Display(Name = "Powiązane Numery")]
        public string RelatedNumbers { get; set; }


        [Display(Name = "Lp")]
        public int? ordinalNumber { get; set; }

        [Display(Name = "ukryj ukończ.")]
        public bool IsExecuted { get; set; } = true;

        [Display(Name = "tylko moje")]
        public bool MyProjects { get; set; } = false;

        [Display(Name = "tylko nowe")]
        public bool ShowProjectsWithNotifications { get; set; } = false;

        [Display(Name = "Kierownik Projektu")]
        public string ProjectManagerId { get; set; }
        
        [Display(Name = "Kto Utworzył")]
        public string UserId { get; set; }

        [Display(Name = "Uczestnik projektu")]
        public string ProjectTeamMemberId { get; set; }

        [Display(Name = "Autor")]
        public string AuthorId { get; set; }

        [Display(Name = "Rok")]
        public int Year { get; set; } = DateTime.Now.Year;
    }
}
