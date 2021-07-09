using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class ProjectRiskService : IProjectRiskService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProjectRiskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       
        public ProjectRisk GetProjectRisk(int projectId, int id, string userId)
        {
            return _unitOfWork.ProjectRiskRepository.GetProjectRisk(projectId, id, userId);
        }

        public ProjectRisk NewProjectRisk(int projectId, string userId)
        {
            return _unitOfWork.ProjectRiskRepository.NewProjectRisk(projectId, userId);
        }

        public void AddProjectRisk(ProjectRisk projectRisk, string userId)
        {
            _unitOfWork.ProjectRiskRepository.AddProjectRisk(projectRisk, userId);
            _unitOfWork.Complete();
        }

        public void UpdateProjectRisk(ProjectRisk projectRisk, string userId)
        {
            _unitOfWork.ProjectRiskRepository.UpdateProjectRisk(projectRisk, userId);
            _unitOfWork.Complete();
        }

        public void DeleteProjectRisk(int projectId, int id, string userId)
        {
            _unitOfWork.ProjectRiskRepository.DeleteProjectRisk(projectId, id, userId);
            _unitOfWork.Complete();
        }
    }
}
