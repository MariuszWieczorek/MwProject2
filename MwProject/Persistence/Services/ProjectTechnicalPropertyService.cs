using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class ProjectTechnicalPropertyService : IProjectTechnicalPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProjectTechnicalPropertyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ProjectTechnicalProperty GetProjectTechnicalProperty(int projectId, int id, string userId)
        {
            return _unitOfWork.ProjectTechnicalPropertyRepository.GetProjectTechnicalProperty(projectId, id, userId);
        }

        public ProjectTechnicalProperty NewProjectTechnicalProperty(int projectId, string userId)
        {
            return _unitOfWork.ProjectTechnicalPropertyRepository.NewProjectTechnicalProperty(projectId, userId);
        }

        public void AddProjectTechnicalProperty(ProjectTechnicalProperty projectTechnicalProperty, string userId)
        {
            _unitOfWork.ProjectTechnicalPropertyRepository.AddProjectTechnicalProperty(projectTechnicalProperty,userId);
            _unitOfWork.Complete();
        }

        public void UpdateProjectTechnicalProperty(ProjectTechnicalProperty projectTechnicalProperty, string userId)
        {
            _unitOfWork.ProjectTechnicalPropertyRepository.UpdateProjectTechnicalProperty(projectTechnicalProperty, userId);
            _unitOfWork.Complete();

        }

        public void DeleteProjectTechnicalProperty(int projectId, int id, string userId)
        {
            _unitOfWork.ProjectTechnicalPropertyRepository.DeleteProjectTechnicalProperty(projectId, id, userId);
            _unitOfWork.Complete();
        }
    }
}
