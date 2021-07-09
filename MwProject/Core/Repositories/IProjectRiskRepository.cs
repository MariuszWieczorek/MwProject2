using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Repositories
{
    public interface IProjectRiskRepository
    {
        ProjectRisk GetProjectRisk(int projectId, int id, string userId);
        ProjectRisk NewProjectRisk(int projectId, string userId);
        void AddProjectRisk(ProjectRisk projectRisk, string userId);
        void UpdateProjectRisk(ProjectRisk projectRisk, string userId);
        void DeleteProjectRisk(int projectId, int id, string userId);
    }
}
