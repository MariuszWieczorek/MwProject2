using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class RequirementService : IRequirementService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RequirementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddRequirement(Requirement requirement)
        {
            _unitOfWork.Requirement.AddRequirement(requirement);
            _unitOfWork.Complete();
        }

        public void DeleteRequirement(int id)
        {
            _unitOfWork.Requirement.DeleteRequirement(id);
            _unitOfWork.Complete();
        }

        public IEnumerable<Requirement> GetRequirements()
        {
            return _unitOfWork.Requirement.GetRequirements();
        }

        public void UpdateRequirement(Requirement requirement)
        {
            _unitOfWork.Requirement.UpdateRequirement(requirement);
            _unitOfWork.Complete();
        }

        public Requirement GetRequirement(int id)
        {
            return _unitOfWork.Requirement.GetRequirement(id);
        }

        public Requirement NewRequirement()
        {
            return _unitOfWork.Requirement.NewRequirement();
        }

        public void SetIsActiveToFalse(int id)
        {
            _unitOfWork.Requirement.SetIsActiveToFalse(id);
            _unitOfWork.Complete();

        }
    }
}
