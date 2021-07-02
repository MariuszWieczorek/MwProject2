using MwProject.Core.Models;
using MwProject.Core.Models.Domains;
using MwProject.Core.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.ViewModels
{
    public class ProjectsViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public ProjectsFilter ProjectsFilter { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public IEnumerable<ProductGroup> ProductGroups { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }

        public IEnumerable<ProjectStatus> ProjectStatuses { get; set; }
        public IEnumerable<ProjectGroup> ProjectGroups { get; set; }
    }
}
