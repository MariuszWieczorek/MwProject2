using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class EstimatedSalesValue
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        
        [Display(Name = "rok")]
        [Required(ErrorMessage = "Pole rok jest wymagane.")]
        [Column(TypeName = "int")]
        public int Year { get; set; }
        
        [Display(Name = "szacowana ilosc")]
        [Required(ErrorMessage = "Pole szacowana ilość jest wymagane.")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Qty  { get; set; }
        [Display(Name = "szacowana cena")]
        [Required(ErrorMessage = "Pole szacowana cena jest wymagane.")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public Project Project { get; set; }
    }
}
