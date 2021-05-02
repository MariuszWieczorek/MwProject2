using MwProject.Core.Models;
using MwProject.Core.Models.Domains;
using MwProject.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Services
{
    public interface IProjectService
    {
        IEnumerable<Project> GetProjects(ProjectsFilter projectsFilter, PagingInfo pagingInfo, int categoryId, string userId);
        Project GetProject(int id, string userId);
        Project NewProject(string userId);
        void AddProject(Project project);
        void UpdateProject(Project project, string userId);
        void FinishProject(int id, string userId);
        void DeleteProject(int id, string userId);
        int GetNumberOfRecords(ProjectsFilter projectFilter, int categoryId);
        void AddTechnicalPropertiesToProject(Project project);
        void AddQualityRequirementsToProject(Project project);
        void AddEconomicRequirementsToProject(Project project);
        IEnumerable<Category> GetUsedCategories();

    }
}
