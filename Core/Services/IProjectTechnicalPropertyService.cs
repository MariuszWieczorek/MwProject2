using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Services
{
    public interface IProjectTechnicalPropertyService
    {
        ProjectTechnicalProperty GetProjectTechnicalProperty(int projectId, int id, string userId);
        ProjectTechnicalProperty NewProjectTechnicalProperty(int projectId, string userId);
        void AddProjectTechnicalProperty(ProjectTechnicalProperty projectTechnicalProperty, string userId);
        void UpdateProjectTechnicalProperty(ProjectTechnicalProperty projectTechnicalProperty, string userId);
        void DeleteProjectTechnicalProperty(int projectId, int id, string userId);
    }
}
