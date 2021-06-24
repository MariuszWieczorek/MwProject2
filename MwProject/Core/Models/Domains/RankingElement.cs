using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class RankingElement
    {

        public RankingElement()
        {
            PurposeOfTheProjects = new Collection<Project>();
            ViabilityOfTheProjects = new Collection<Project>();
            CompetitivenessOfTheProjects = new Collection<Project>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Zakres od ..")]
        public decimal RangeFrom { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Zakres do ..")]
        public decimal RangeTo { get; set; }

        [Display(Name = "indeks")]
        public int Index { get; set; }
        
        
        [Display(Name = "Kategoria")]
        public int RankingCategoryId { get; set; }
        public RankingCategory RankingCategory { get; set; }

        
        [InverseProperty("PurposeOfTheProject")]
        public ICollection<Project> PurposeOfTheProjects { get; set; }


        [InverseProperty("ViabilityOfTheProject")]
        public ICollection<Project> ViabilityOfTheProjects { get; set; }


        [InverseProperty("CompetitivenessOfTheProject")]
        public ICollection<Project> CompetitivenessOfTheProjects { get; set; }
    }
}
