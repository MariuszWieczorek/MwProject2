using Microsoft.EntityFrameworkCore;
using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MwProject.Persistence.Repositories
{
    public class ProjectRiskRepository : IProjectRiskRepository
    {
        private readonly IApplicationDbContext _context;
        public ProjectRiskRepository(IApplicationDbContext context)
        {
            _context = context;
        }
                
        public ProjectRisk GetProjectRisk(int projectId, int id, string userId)
        {
            var projectRisk = _context.ProjectRisks.Single(x => x.ProjectId == projectId && x.Id == id);
            return projectRisk;
        }

        public ProjectRisk NewProjectRisk(int projectId, string userId)
        {
            int ordinalNumber = 1;
            if (_context.ProjectRisks.Where(x => x.ProjectId == projectId).Any())
            {
                ordinalNumber = _context.ProjectRisks.Where(x => x.ProjectId == projectId).Max(x => x.OrdinalNumber) + 1;
            }

            return new ProjectRisk()
            {
                OrdinalNumber = ordinalNumber,
                ProjectId = projectId
            };
        }

        public void AddProjectRisk(ProjectRisk projectRisk, string userId)
        {
            _context.ProjectRisks.Add(projectRisk);
        }

 
        public void UpdateProjectRisk(ProjectRisk projectRisk, string userId)
        {
            var projectRiskToUpdate = _context.ProjectRisks
                .Single(x => x.ProjectId == projectRisk.ProjectId && x.Id == projectRisk.Id);

            projectRiskToUpdate.Name = projectRisk.Name;
            projectRiskToUpdate.Content = projectRisk.Content;

        }

        public void DeleteProjectRisk(int projectId, int id, string userId)
        {
            var projectRiskToDelete = _context.ProjectRisks.Single(x => x.ProjectId == projectId && x.Id == id);
            _context.ProjectRisks.Remove(projectRiskToDelete);
        }

    }
}
