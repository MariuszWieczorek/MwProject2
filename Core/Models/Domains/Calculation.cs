using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class Calculation
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }

        // koszty
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Koszt Materiałów PLN")]
        public decimal MaterialCosts { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Koszt Robocizny PLN")]
        public decimal LabourCosts { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Koszty Narzutu PLN")]
        public decimal Markup { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Koszty Pakowania PLN")]
        public decimal PackingCosts { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Koszty Ogólne [%]")]
        public decimal GeneralCostsInPercent { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "TKW")]
        public decimal Tkw { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "CKW")]
        public decimal Ckw { get; set; }

        [Display(Name = "Komentarz")]
        public string Comment { get; set; }
        [Display(Name = "przygotował dane")]
        public string createdBy { get; set; }
        public DateTime createdDate { get; set; }
        [Display(Name = "potwierdził dane")]
        public string confirmedBy { get; set; }
        public DateTime confirmedDate { get; set; }
        public Project Project { get; set; }
    }
}
