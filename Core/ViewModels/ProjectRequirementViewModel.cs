using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.ViewModels
{
    public class ProjectRequirementViewModel
    {
        public string Heading { get; set; }
        public ProjectRequirement ProjectRequirement { get; set; }
        public IEnumerable<Requirement> Requirements { get; set; }
    }
}
