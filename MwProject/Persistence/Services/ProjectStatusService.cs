using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class ProjectStatusService : IProjectStatusService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProjectStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ProjectStatus> GetProjectStatuses()
        {
            return _unitOfWork.ProjectStatusRepository.GetProjectStatuses();
        }

        public void AddProjectStatus(ProjectStatus projectStatus)
        {
            _unitOfWork.ProjectStatusRepository.AddProjectStatus(projectStatus);
            _unitOfWork.Complete();
        }

        public ProjectStatus GetProjectStatus(int id)
        {
            return _unitOfWork.ProjectStatusRepository.GetProjectStatus(id);
        }

        public void UpdateProjectStatus(ProjectStatus projectStatus)
        {
            _unitOfWork.ProjectStatusRepository.UpdateProjectStatus(projectStatus);
            _unitOfWork.Complete();
        }

        public void DeleteProjectStatus(int id)
        {
            _unitOfWork.ProjectStatusRepository.DeleteProjectStatus(id);
            _unitOfWork.Complete();
        }
    }
}
