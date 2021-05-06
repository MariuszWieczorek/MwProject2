using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.ViewModels
{
    public class RequirementsViewModel
    {
        public IEnumerable<Requirement> Requirements { get; set; }
        public ApplicationUser CurrentUser { get; set; }
    }
}
