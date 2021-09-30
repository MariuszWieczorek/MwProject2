using Microsoft.EntityFrameworkCore;
using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MwProject.Persistence.Repositories
{
    public class ProjectTeamMemberRepository : IProjectTeamMemberRepository
    {
        private readonly IApplicationDbContext _context;
        public ProjectTeamMemberRepository(IApplicationDbContext context)
        {
            _context = context;
        }
                
        public ProjectTeamMember GetProjectTeamMember(int projectId, int id, string userId)
        {
            var projectTeamMember = _context.ProjectTeamMembers.Single(x => x.ProjectId == projectId && x.Id == id);
            return projectTeamMember;
        }


        public IEnumerable<ProjectTeamMember> GetProjectTeamMembers()
        {
            var projectTeamMembers = _context.ProjectTeamMembers
                .Include(x => x.User)
                .Include(x => x.Project)
                .Where(x=>x.Project.IsExecuted == false)
                .ToList();

            return projectTeamMembers;
        }

        public ProjectTeamMember NewProjectTeamMember(int projectId, string userId)
        {
            int ordinalNumber = 1;
            if (_context.ProjectTeamMembers.Where(x => x.ProjectId == projectId).Any())
            {
                ordinalNumber = _context.ProjectTeamMembers.Where(x => x.ProjectId == projectId).Max(x => x.OrdinalNumber) + 1;
            }

            return new ProjectTeamMember()
            {
                OrdinalNumber = ordinalNumber,
                ProjectId = projectId
            };
        }

        public void AddProjectTeamMember(ProjectTeamMember projectTeamMember, string userId)
        {
            _context.ProjectTeamMembers.Add(projectTeamMember);
        }

        public void UpdateProjectTeamMember(ProjectTeamMember projectTeamMember, string userId)
        {
            var projectTeamMemberToUpdate = _context.ProjectTeamMembers
                .Single(x => x.ProjectId == projectTeamMember.ProjectId && x.Id == projectTeamMember.Id);

            projectTeamMemberToUpdate.UserId = projectTeamMember.UserId;
            projectTeamMemberToUpdate.Description = projectTeamMember.Description;

        }

        public void DeleteProjectTeamMember(int projectId, int id, string userId)
        {
            var projectTeamMemberToDelete = _context.ProjectTeamMembers.Single(x => x.ProjectId == projectId && x.Id == id);
            _context.ProjectTeamMembers.Remove(projectTeamMemberToDelete);
        }
    }
}
