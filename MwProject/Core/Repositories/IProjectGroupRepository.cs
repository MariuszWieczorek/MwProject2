using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Repositories
{
    public interface IProjectGroupRepository
    {
        IEnumerable<ProjectGroup> GetProjectGroups();
        void AddProjectGroup(ProjectGroup projectGroup);
        ProjectGroup NewProjectGroup();
        ProjectGroup GetProjectGroup(int id);
        void UpdateProjectGroup(ProjectGroup projectGroup);
        void DeleteProjectGroup(int id);

    }
}
