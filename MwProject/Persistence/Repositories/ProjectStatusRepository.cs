using Microsoft.EntityFrameworkCore;
using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MwProject.Persistence.Repositories
{
    public class ProjectStatusRepository : IProjectStatusRepository
    {
        private readonly IApplicationDbContext _context;
        public ProjectStatusRepository(IApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<ProjectStatus> GetProjectStatuses()
        {
            return _context.ProjectStatuses
                .OrderBy(x => x.OrdinalNumber)
                .ToList();
        }

        public void AddProjectStatus(ProjectStatus projectStatus)
        {
            _context.ProjectStatuses.Add(projectStatus);
        }

        public ProjectStatus GetProjectStatus(int id)
        {
            var projectStatus = _context.ProjectStatuses.Single(x => x.Id == id);
            return projectStatus;
        }

        public void UpdateProjectStatus(ProjectStatus projectStatus)
        {
            var projectStatusToUpdate = _context.ProjectStatuses.Single(x => x.Id == projectStatus.Id);
            projectStatusToUpdate.Name = projectStatus.Name; 
        }

        public void DeleteProjectStatus(int id)
        {
            var projectStatusToDelete = _context.ProjectStatuses.Single(x => x.Id == id);
            _context.ProjectStatuses.Remove(projectStatusToDelete);
        }
    }
}
