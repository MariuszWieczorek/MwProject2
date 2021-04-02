using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class ApplicationUser : IdentityUser
    {
        
        public ApplicationUser()
        {
            Projects = new Collection<Project>();
        }
        public ICollection<Project> Projects;
        
    }
}
