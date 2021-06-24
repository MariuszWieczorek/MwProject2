using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class CategoryRequirementService : ICategoryRequirementService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryRequirementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CategoryRequirement GetCategoryRequirement(int categoryId, int id, string userId)
        {
            return _unitOfWork.CategoryRequirementRepository.GetCategoryRequirement(categoryId, id, userId);
        }

        public CategoryRequirement NewCategoryRequirement(int categoryId, string userId, int type)
        {
            return _unitOfWork.CategoryRequirementRepository.NewCategoryRequirement(categoryId, userId, type);
        }

        public void AddCategoryRequirement(CategoryRequirement categoryRequirement, string userId)
        {
            _unitOfWork.CategoryRequirementRepository.AddCategoryRequirement(categoryRequirement, userId);
            _unitOfWork.Complete();
        }

        public void UpdateCategoryRequirement(CategoryRequirement categoryRequirement, string userId)
        {
            _unitOfWork.CategoryRequirementRepository.UpdateCategoryRequirement(categoryRequirement, userId);
            _unitOfWork.Complete();
        }

        public void DeleteCategoryRequirement(int categoryId, int id, string userId)
        {
            _unitOfWork.CategoryRequirementRepository.DeleteCategoryRequirement(categoryId, id, userId);
            _unitOfWork.Complete();
        }
    }
}
