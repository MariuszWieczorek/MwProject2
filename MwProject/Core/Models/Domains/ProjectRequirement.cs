using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class ProjectRequirement
    {
        public int Id { get; set; }

        [Column(TypeName = "tinyint")]
        [Display(Name = "Pokaż tekst: Tak/Nie")]
        public int YesNo { get; set; }

        [Display(Name = "Czy występuje?")]
        public bool Exist { get; set; }


        [Display(Name = "Pokaż Wartość")]
        public bool ShowValue { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Koszt PLN")]
        public decimal Value { get; set; }

        [Display(Name = "Opis")]
        public string Comment { get; set; }

        [Display(Name = "Link")]
        public string Link { get; set; }

        [Display(Name = "Powiązane Numery")]
        public string RelatedNumbers { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }


        [Display(Name = "Parametr")]
        public int RequirementId { get; set; }
        public Requirement Requirement { get; set; }
    }
}
