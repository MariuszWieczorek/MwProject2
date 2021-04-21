using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Persistence.Repositories
{
    public class ProjectRequirementRepository : IProjectRequirementRepository
    {

        private readonly IApplicationDbContext _context;
        public ProjectRequirementRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public void AddProjectRequirement(ProjectRequirement projectRequirement, string userId)
        {
            _context.ProjectRequirements.Add(projectRequirement);
        }

        public void DeleteProjectRequirement(int projectId, int id, string userId)
        {
            var projectRequirementToDelete = _context.ProjectRequirements.Single(x => x.ProjectId==projectId && x.Id == id);
            _context.ProjectRequirements.Remove(projectRequirementToDelete);

        }

        public ProjectRequirement GetProjectRequirement(int projectId, int id, string userId)
        {
            return _context.ProjectRequirements.Single(x => x.ProjectId == projectId && x.Id == id);
        }

        public ProjectRequirement NewProjectRequirement(int projectId, string userId)
        {
            return new ProjectRequirement
            {
                ProjectId = projectId,
                Exist = true
            };
        }

        public void UpdateProjectRequirement(ProjectRequirement projectRequirement, string userId)
        {
            var projectRequirementToUpdate = _context.ProjectRequirements
                .Single(x => x.ProjectId == projectRequirement.ProjectId && x.Id == projectRequirement.Id);
            projectRequirementToUpdate.RequirementId = projectRequirement.RequirementId;
            projectRequirementToUpdate.Exist = projectRequirement.Exist;
            projectRequirementToUpdate.Value = projectRequirement.Value;
            projectRequirementToUpdate.Comment = projectRequirement.Comment;
        }
    }
}
