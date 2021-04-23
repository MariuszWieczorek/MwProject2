﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class TechnicalProperty
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Pole nazwa jest wymagane.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        
        public int ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }
    }
}
