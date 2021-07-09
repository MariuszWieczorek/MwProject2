using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.ViewModels
{
    public class ProjectRiskViewModel
    {
        public string Heading { get; set; }
        public ProjectRisk ProjectRisk { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
        public ApplicationUser CurrentUser { get; set; }
    }
}
