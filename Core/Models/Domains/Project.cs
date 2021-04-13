﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class Project
    {
        public int Id { get; set; }
       
        [MaxLength(50)]
        [Required(ErrorMessage = "Pole tytuł jest wymagane.")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Pole opis jest wymagane.")]
        [Display(Name = "Opis")]
        public string Description { get; set; }
        
        [Display(Name = "Data Utworzenia")]
        public DateTime? CreatedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Display(Name = "Termin")]
        public DateTime? Term { get; set; }

        [Required(ErrorMessage = "Pole wartość jest wymagane.")]
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
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

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
        public string CommentofCost { get; set; }
    }
}
