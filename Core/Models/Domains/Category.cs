﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class Category
    {
        public Category()
        {
            Projects = new Collection<Project>();
        }
        public int Id { get; set; }

        [Required]
        [Display(Name = "Lp")]
        public int OrdinalNumber { get; set; }

        [Required]
        [Display(Name = "Nazwa kategorii")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Symbol Dokumentu")]
        public string DocumentSymbol { get; set; }

        [Required]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        public ICollection<Project> Projects;

    }
}
