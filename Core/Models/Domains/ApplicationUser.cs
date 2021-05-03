using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class ApplicationUser : IdentityUser
    {

        [Required]
        [Display(Name = "Imię")]
        public string  FirstName { get; set; }

        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        public ApplicationUser()
        {
            Projects = new Collection<Project>();
        }
        public ICollection<Project> Projects;
        
    }
}
