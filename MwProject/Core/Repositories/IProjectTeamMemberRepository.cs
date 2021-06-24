using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Repositories
{
    public interface IProjectTeamMemberRepository
    {
        ProjectTeamMember GetProjectTeamMember(int projectId, int id, string userId);
        ProjectTeamMember NewProjectTeamMember(int projectId, string userId);
        void AddProjectTeamMember(ProjectTeamMember projectTeamMember, string userId);
        void UpdateProjectTeamMember(ProjectTeamMember projectTeamMember, string userId);
        void DeleteProjectTeamMember(int projectId, int id, string userId);
    }
}
