using MwProject.Core.Models;
using MwProject.Core.Models.Domains;
using MwProject.Core.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.ViewModels
{
    public class ProjectsStatisticsViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<ProjectStatus> ProjectStatuses { get; set; }
        public IEnumerable<ProjectGroup> ProjectGroups { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
        public int TotalCount { get; set; }
        public int Mission50Count { get; set; }
        public int PolishAreaCount { get; set; }
    }
}
