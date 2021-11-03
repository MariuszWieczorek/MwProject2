using MwProject.Core.Models.Domains;
using MwProject.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.ViewModels
{
    public class ProjectRequirementViewModel
    {
        public string Heading { get; set; }
        public RequirementType RequirementType { get; set; }
        public ProjectRequirement ProjectRequirement { get; set; }
        public IEnumerable<Requirement> Requirements { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public string Tab { get; set; }


    }
}
