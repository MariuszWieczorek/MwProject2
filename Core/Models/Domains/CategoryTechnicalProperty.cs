using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class CategoryTechnicalProperty
    {
        public int Id { get; set; }

        [Display(Name = "Lp")]
        public int OrdinalNumber { get; set; }

        [Display(Name = "Czy występuje?")]
        public bool Exist { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Wartość")]
        public decimal Value { get; set; }

        [Display(Name = "Komentarz")]
        public string Comment { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int TechnicalPropertyId { get; set; }
        public TechnicalProperty TechnicalProperty { get; set; }
    }
}
