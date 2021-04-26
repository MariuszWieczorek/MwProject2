using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.ViewModels
{
    public class ProjectTechnicalPropertyViewModel
    {
        public string Heading { get; set; }
        public ProjectTechnicalProperty ProjectTechnicalProperty { get; set; }
        public IEnumerable<TechnicalProperty> TechnicalProperties { get; set; }
    }
}
