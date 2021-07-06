using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class ProjectClientService : IProjectClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProjectClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       
        public ProjectClient GetProjectClient(int projectId, int id, string userId)
        {
            return _unitOfWork.ProjectClientRepository.GetProjectClient(projectId, id, userId);
        }

        public ProjectClient NewProjectClient(int projectId, string userId)
        {
            return _unitOfWork.ProjectClientRepository.NewProjectClient(projectId, userId);
        }

        public void AddProjectClient(ProjectClient projectClient, string userId)
        {
            _unitOfWork.ProjectClientRepository.AddProjectClient(projectClient, userId);
            _unitOfWork.Complete();
        }

        public void UpdateProjectClient(ProjectClient projectClient, string userId)
        {
            _unitOfWork.ProjectClientRepository.UpdateProjectClient(projectClient, userId);
            _unitOfWork.Complete();
        }

        public void DeleteProjectClient(int projectId, int id, string userId)
        {
            _unitOfWork.ProjectClientRepository.DeleteProjectClient(projectId, id, userId);
            _unitOfWork.Complete();
        }
    }
}
