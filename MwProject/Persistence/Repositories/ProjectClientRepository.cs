using Microsoft.EntityFrameworkCore;
using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MwProject.Persistence.Repositories
{
    public class ProjectClientRepository : IProjectClientRepository
    {
        private readonly IApplicationDbContext _context;
        public ProjectClientRepository(IApplicationDbContext context)
        {
            _context = context;
        }
                
        public ProjectClient GetProjectClient(int projectId, int id, string userId)
        {
            var projectClient = _context.ProjectClients.Single(x => x.ProjectId == projectId && x.Id == id);
            return projectClient;
        }

        public ProjectClient NewProjectClient(int projectId, string userId)
        {
            int ordinalNumber = 1;
            if (_context.ProjectClients.Where(x => x.ProjectId == projectId).Any())
            {
                ordinalNumber = _context.ProjectClients.Where(x => x.ProjectId == projectId).Max(x => x.OrdinalNumber) + 1;
            }

            return new ProjectClient()
            {
                OrdinalNumber = ordinalNumber,
                ProjectId = projectId
            };
        }

        public void AddProjectClient(ProjectClient projectClient, string userId)
        {
            _context.ProjectClients.Add(projectClient);
        }

 
        public void UpdateProjectClient(ProjectClient projectClient, string userId)
        {
            var projectClientToUpdate = _context.ProjectClients
                .Single(x => x.ProjectId == projectClient.ProjectId && x.Id == projectClient.Id);

            projectClientToUpdate.Name = projectClient.Name;

        }

        public void DeleteProjectClient(int projectId, int id, string userId)
        {
            var projectClientToDelete = _context.ProjectClients.Single(x => x.ProjectId == projectId && x.Id == id);
            _context.ProjectClients.Remove(projectClientToDelete);
        }

    }
}
