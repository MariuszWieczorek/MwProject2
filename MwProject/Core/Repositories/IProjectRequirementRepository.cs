using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Repositories
{
    public interface IProjectRequirementRepository
    {
        ProjectRequirement GetProjectRequirement(int projectId, int id, string userId);
        ProjectRequirement NewProjectRequirement(int projectId, string userId);
        void AddProjectRequirement(ProjectRequirement projectRequirement, string userId);
        void UpdateProjectRequirement(ProjectRequirement projectRequirement, string userId);
        void DeleteProjectRequirement(int projectId, int id, string userId);
    }
}
