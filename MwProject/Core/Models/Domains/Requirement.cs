using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class Requirement
    {
        public Requirement()
        {
            ProjectRequirements = new Collection<ProjectRequirement>();
            CategoryRequirements = new Collection<CategoryRequirement>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Lp")]
        public int OrdinalNumber { get; set; }

        [Required(ErrorMessage = "Pole nazwa jest wymagane.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        public int Type { get; set; }
        
        [Display(Name = "Widoczny")]
        public bool IsActive { get; set; }

        public ICollection<ProjectRequirement> ProjectRequirements;
        
        public ICollection<CategoryRequirement> CategoryRequirements;
    }

}
