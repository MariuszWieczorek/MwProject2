using Microsoft.EntityFrameworkCore;
using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MwProject.Persistence.Repositories
{
    public class ProjectGroupRepository : IProjectGroupRepository
    {
        private readonly IApplicationDbContext _context;
        public ProjectGroupRepository(IApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<ProjectGroup> GetProjectGroups()
        {
            return _context.ProjectGroups
                .OrderBy(x => x.OrdinalNumber)
                .ToList();
        }

        public void AddProjectGroup(ProjectGroup projectGroup)
        {
            _context.ProjectGroups.Add(projectGroup);
        }

        public ProjectGroup GetProjectGroup(int id)
        {
            var projectGroup = _context.ProjectGroups.Single(x => x.Id == id);
            return projectGroup;
        }

        public void UpdateProjectGroup(ProjectGroup projectGroup)
        {
            var projectGroupToUpdate = _context.ProjectGroups.Single(x => x.Id == projectGroup.Id);
            projectGroupToUpdate.Name = projectGroup.Name; 
        }

        public void DeleteProjectGroup(int id)
        {
            var projectGroupToDelete = _context.ProjectGroups.Single(x => x.Id == id);
            _context.ProjectGroups.Remove(projectGroupToDelete);
        }

        public ProjectGroup NewProjectGroup()
        {
            int ordinalNumber = 1;
            if (_context.ProjectGroups.Any())
            {
                ordinalNumber = _context.ProjectGroups.Max(x => x.OrdinalNumber) + 1;
            }

            return new ProjectGroup
            {
                Name = "",
                OrdinalNumber = ordinalNumber
            };
        }
    }
}
