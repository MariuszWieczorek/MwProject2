using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class ProjectTeamMemberService : IProjectTeamMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProjectTeamMemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       
        public ProjectTeamMember GetProjectTeamMember(int projectId, int id, string userId)
        {
            return _unitOfWork.ProjectTeamMemberRepository.GetProjectTeamMember(projectId, id, userId);
        }

        public ProjectTeamMember NewProjectTeamMember(int projectId, string userId)
        {
            return _unitOfWork.ProjectTeamMemberRepository.NewProjectTeamMember(projectId, userId);
        }

        public void AddProjectTeamMember(ProjectTeamMember projectTeamMember, string userId)
        {
            _unitOfWork.ProjectTeamMemberRepository.AddProjectTeamMember(projectTeamMember, userId);
            _unitOfWork.Complete();
        }

        public void UpdateProjectTeamMember(ProjectTeamMember projectTeamMember, string userId)
        {
            _unitOfWork.ProjectTeamMemberRepository.UpdateProjectTeamMember(projectTeamMember, userId);
            _unitOfWork.Complete();
        }

        public void DeleteProjectTeamMember(int projectId, int id, string userId)
        {
            _unitOfWork.ProjectTeamMemberRepository.DeleteProjectTeamMember(projectId, id, userId);
            _unitOfWork.Complete();
        }
    }
}
