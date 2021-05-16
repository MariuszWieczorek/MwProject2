using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class RankingCategory
    {
        public RankingCategory()
        {
            RankingElements = new Collection<RankingElement>();
        }

        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Skrót")]
        public string Abbrev { get; set; }

        public ICollection<RankingElement> RankingElements { get; set; }

            
    }
}
