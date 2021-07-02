using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class ProjectGroupService : IProjectGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProjectGroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ProjectGroup> GetProjectGroups()
        {
            return _unitOfWork.ProjectGroupRepository.GetProjectGroups();
        }

        public void AddProjectGroup(ProjectGroup projectGroup)
        {
            _unitOfWork.ProjectGroupRepository.AddProjectGroup(projectGroup);
            _unitOfWork.Complete();
        }

        public ProjectGroup GetProjectGroup(int id)
        {
            return _unitOfWork.ProjectGroupRepository.GetProjectGroup(id);
        }

        public void UpdateProjectGroup(ProjectGroup projectGroup)
        {
            _unitOfWork.ProjectGroupRepository.UpdateProjectGroup(projectGroup);
            _unitOfWork.Complete();
        }

        public void DeleteProjectGroup(int id)
        {
            _unitOfWork.ProjectGroupRepository.DeleteProjectGroup(id);
            _unitOfWork.Complete();
        }

        public ProjectGroup NewProjectGroup()
        {
            return _unitOfWork.ProjectGroupRepository.NewProjectGroup();
        }
    }
}
