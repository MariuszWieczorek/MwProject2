using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            ProjectTechnicalProperties = new Collection<ProjectTechnicalProperty>();
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
        [Display(Name = "Oczekiwany Termin Realizacji")]
        public DateTime? Term { get; set; }

        [Required(ErrorMessage = "Pole zainicjowane przez jest wymagane.")]
        [Display(Name = "Wniosek zainicjowany przez")]
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


        [Required(ErrorMessage = "Pole grupa produktu jest wymagane.")]
        [Display(Name = "Grupa Produktu")]
        public int ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }


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
        public ICollection<ProjectTechnicalProperty> ProjectTechnicalProperties { get; set; }
    }
}
