using Microsoft.EntityFrameworkCore;
using MwProject.Core;
using MwProject.Core.Models;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using MwProject.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {

        private readonly IApplicationDbContext _context;
        public ProjectRepository(IApplicationDbContext context)
        {
            _context = context;
        }


        public void AddProject(Project project)
        {
            _context.Projects.Add(project);
        }

        public void DeleteProject(int id, string userId)
        {
            var projectToDelete = _context.Projects.Single(x => x.Id == id && x.UserId == userId);
            _context.Projects.Remove(projectToDelete);
        }

        public void FinishProject(int id, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == id && x.UserId == userId);
            projectToUpdate.IsExecuted = true;
        }

        public Project GetProject(int id, string userId)
        {
            var project = _context.Projects.Single(x => x.Id == id && x.UserId == userId);
            return project;
        }

        public IEnumerable<Project> GetProjects(ProjectsFilter projectsFilter, PagingInfo pagingInfo, int categoryId, string userId)
        {
            var projects = _context.Projects
                .Include(x => x.Category)
                .Where(x => x.UserId == userId && x.IsExecuted == projectsFilter.IsExecuted);

            if (projectsFilter.CategoryId != 0)
                projects = projects.Where(x => x.CategoryId == projectsFilter.CategoryId);

            if (!string.IsNullOrWhiteSpace(projectsFilter.Title))
                projects = projects.Where(x => x.Title.Contains(projectsFilter.Title));

            if (pagingInfo != null)
            {
                projects = projects
                    .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.ItemsPerPage)
                    .Take(pagingInfo.ItemsPerPage);
            }

            return projects.OrderBy(x => x.Term).ToList();
        }


        public int GetNumberOfRecords(ProjectsFilter projectFilter, int categoryId)
        {
            var projects = _context.Projects
                .Include(x => x.Category).AsQueryable();

            if (projectFilter.IsExecuted == true)
                projects = projects.Where(x => x.IsExecuted == projectFilter.IsExecuted);

            if (projectFilter.CategoryId != 0)
                projects = projects.Where(x => x.CategoryId == projectFilter.CategoryId);

            if (categoryId != 0)
                projects = projects.Where(x => x.CategoryId == categoryId);

            if (!string.IsNullOrWhiteSpace(projectFilter.Title))
                projects = projects.Where(x => x.Title.Contains(projectFilter.Title));


            return projects.Count();
        }

        public IEnumerable<Category> GetUsedCategories()
        {
            var categories = _context.Projects
                .Include(x => x.Category)
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x);

            return categories;
        }

        public void UpdateProject(Project project, string userId)
        {
            var projectToUpdate = _context.Projects.Single(x => x.Id == project.Id && x.UserId == userId);
            projectToUpdate.Description = project.Description;
            projectToUpdate.Title = project.Title;
            projectToUpdate.CategoryId = project.CategoryId;
            projectToUpdate.Term = project.Term;
        }
    }
}
