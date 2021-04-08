using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.ViewModels
{
    public class ProjectViewModel
    {
        public string Heading { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public Project Project { get; set; }

    }
}
