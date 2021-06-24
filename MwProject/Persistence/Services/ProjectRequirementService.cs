using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class ProjectRequirementService : IProjectRequirementService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProjectRequirementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ProjectRequirement GetProjectRequirement(int projectId, int id, string userId)
        {
            return _unitOfWork.ProjectRequirement.GetProjectRequirement(projectId, id, userId);
        }

        public ProjectRequirement NewProjectRequirement(int projectId, string userId)
        {
            return _unitOfWork.ProjectRequirement.NewProjectRequirement(projectId, userId);
        }

        public void AddProjectRequirement(ProjectRequirement projectRequirement, string userId)
        {
            _unitOfWork.ProjectRequirement.AddProjectRequirement(projectRequirement, userId);
            _unitOfWork.Complete();
        }

        public void UpdateProjectRequirement(ProjectRequirement projectRequirement, string userId)
        {
            _unitOfWork.ProjectRequirement.UpdateProjectRequirement(projectRequirement, userId);
            _unitOfWork.Complete();
        }

        public void DeleteProjectRequirement(int projectId, int id, string userId)
        {
            _unitOfWork.ProjectRequirement.DeleteProjectRequirement(projectId, id, userId);
            _unitOfWork.Complete();
        }
    }
}
