using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Repositories
{
    public interface IProjectClientRepository
    {
        ProjectClient GetProjectClient(int projectId, int id, string userId);
        ProjectClient NewProjectClient(int projectId, string userId);
        void AddProjectClient(ProjectClient projectClient, string userId);
        void UpdateProjectClient(ProjectClient projectClient, string userId);
        void DeleteProjectClient(int projectId, int id, string userId);
    }
}
