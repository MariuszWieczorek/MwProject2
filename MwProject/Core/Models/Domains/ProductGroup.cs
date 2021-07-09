using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class ProductGroup
    {
        public ProductGroup()
        {
            Projects = new Collection<Project>();
            Categories = new Collection<Category>();
        }
        public int Id { get; set; }

        [Required]
        [Display(Name = "Lp")]
        public int OrdinalNumber { get; set; }

        [Required]
        [Display(Name = "Nazwa Grupy")]
        public string Name { get; set; }

        public ICollection<Project> Projects;
        public ICollection<Category> Categories;

    }
}
