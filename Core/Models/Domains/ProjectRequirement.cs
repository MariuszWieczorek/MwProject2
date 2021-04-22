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


        [Display(Name = "Czy występuje?")]
        public bool Exist { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Koszt PLN")]
        public decimal Value { get; set; }

        [Display(Name = "Komentarz")]
        public string Comment { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int RequirementId { get; set; }
        public Requirement Requirement { get; set; }
    }
}
