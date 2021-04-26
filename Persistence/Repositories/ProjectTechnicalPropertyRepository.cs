using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Persistence.Repositories
{
    public class ProjectTechnicalPropertyRepository : IProjectTechnicalPropertyRepository
    {

        private readonly IApplicationDbContext _context;
        public ProjectTechnicalPropertyRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public void AddProjectTechnicalProperty(ProjectTechnicalProperty projectTechnicalProperty, string userId)
        {
            _context.ProjectTechnicalProperties.Add(projectTechnicalProperty);
        }

        public void DeleteProjectTechnicalProperty(int projectId, int id, string userId)
        {
            var projectTechnicalPropertyToDelete = _context.ProjectTechnicalProperties.Single(x => x.ProjectId == projectId && x.Id == id);
            _context.ProjectTechnicalProperties.Remove(projectTechnicalPropertyToDelete);
        }

        public ProjectTechnicalProperty GetProjectTechnicalProperty(int projectId, int id, string userId)
        {
            return _context.ProjectTechnicalProperties.Single(x => x.ProjectId == projectId && x.Id == id);
        }

        public ProjectTechnicalProperty NewProjectTechnicalProperty(int projectId, string userId)
        {
            return new ProjectTechnicalProperty
            {
                ProjectId = projectId,
                Exist = true
            };
        }

       
        public void UpdateProjectTechnicalProperty(ProjectTechnicalProperty projectTechnicalProperty, string userId)
        {
            var projectTechnicalPropertyToUpdate = _context.ProjectTechnicalProperties
                .Single(x => x.ProjectId == projectTechnicalProperty.ProjectId && x.Id == projectTechnicalProperty.Id);
            projectTechnicalPropertyToUpdate.TechnicalPropertyId = projectTechnicalProperty.TechnicalPropertyId;
            projectTechnicalPropertyToUpdate.Exist = projectTechnicalProperty.Exist;
            projectTechnicalPropertyToUpdate.Value = projectTechnicalProperty.Value;
            projectTechnicalPropertyToUpdate.Comment = projectTechnicalProperty.Comment;
        }
    }
}
