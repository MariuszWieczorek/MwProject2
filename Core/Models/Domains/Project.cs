using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class Project
    {
        public int Id { get; set; }

        public Project()
        {
            Calculations = new Collection<Calculation>();
            EstimatedSalesValues = new Collection<EstimatedSalesValue>();
            ProjectRequirements = new Collection<ProjectRequirement>();
        }

        [MaxLength(255)]
        [Required(ErrorMessage = "Pole tytuł jest wymagane.")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Pole numer jest wymagane.")]
        [Display(Name = "Numer")]
        public string Number { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }
        
        [Display(Name = "Data Utworzenia")]
        public DateTime? CreatedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Display(Name = "Termin")]
        public DateTime? Term { get; set; }

        [Required(ErrorMessage = "Pole zainicjowane przez jest wymagane.")]
        [Display(Name = "Zainicjowane przez")]
        public string InitiatedBy { get; set; }
        


        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Wartość")]
        public decimal Value { get; set; }

        [MaxLength(250)]
        [Display(Name = "Obrazek")]
        public string Picture { get; set; }


        [Display(Name = "Zrealizowane")]
        public bool IsExecuted { get; set; }

        [Required(ErrorMessage = "Pole kategoria jest wymagane.")]
        [Display(Name = "Kategoria")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Display(Name = "Nowy Produkt")]
        public bool NewProduct { get; set; }


        [Display(Name = "Projekt zaakceptowany przez")]
        public string ConfirmedBy { get; set; }

        [Display(Name = "Czas zaakceptowania projektu")]
        public DateTime ConfirmedDate { get; set; }

        [Display(Name = "Projekt zaakceptowany przez")]
        public string AcceptedBy { get; set; }
        
        [Display(Name = "Czas zaakceptowania projektu")]
        public DateTime AcceptedDate { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Calculation> Calculations { get; set; }
        public ICollection<EstimatedSalesValue> EstimatedSalesValues { get; set; }
        public ICollection<ProjectRequirement> ProjectRequirements { get; set; }
    }
}
