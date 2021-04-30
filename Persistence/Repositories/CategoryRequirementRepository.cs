using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Persistence.Repositories
{
    public class CategoryRequirementRepository : ICategoryRequirementRepository
    {

        private readonly IApplicationDbContext _context;
        public CategoryRequirementRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public void AddCategoryRequirement(CategoryRequirement categoryRequirement, string userId)
        {
            _context.CategoryRequirements.Add(categoryRequirement);
        }


        public void DeleteCategoryRequirement(int categoryId, int id, string userId)
        {
            var categoryRequirementToDelete = _context.CategoryRequirements.Single(x => x.CategoryId == categoryId && x.Id == id);
            _context.CategoryRequirements.Remove(categoryRequirementToDelete);
        }

        public CategoryRequirement GetCategoryRequirement(int categoryId, int id, string userId)
        {
            var categoryRequirement = _context.CategoryRequirements.Single(x => x.CategoryId == categoryId && x.Id == id);
            return categoryRequirement;
        }

        public CategoryRequirement NewCategoryRequirement(int categoryId, string userId, int type)
        {
            int ordinalNumber = 1;
            if(_context.CategoryRequirements.Where(x => x.CategoryId == categoryId && x.Requirement.Type == type).Any())
            {
                ordinalNumber = _context.CategoryRequirements.Where(x => x.CategoryId == categoryId && x.Requirement.Type == type).Max(x => x.OrdinalNumber) + 1;
            }

            return new CategoryRequirement
            {
                CategoryId = categoryId,
                OrdinalNumber = ordinalNumber,
                Exist = true
            };
        }

        public void UpdateCategoryRequirement(CategoryRequirement categoryRequirement, string userId)
        {
            var categoryRequirementToUpdate = _context.CategoryRequirements
                .Single(x => x.CategoryId == categoryRequirement.CategoryId && x.Id == categoryRequirement.Id);

            categoryRequirementToUpdate.RequirementId = categoryRequirement.RequirementId;
            categoryRequirementToUpdate.Exist = categoryRequirement.Exist;
            categoryRequirementToUpdate.Value = categoryRequirement.Value;
            categoryRequirementToUpdate.Comment = categoryRequirement.Comment;
            categoryRequirementToUpdate.OrdinalNumber = categoryRequirement.OrdinalNumber;
            categoryRequirementToUpdate.YesNo = categoryRequirement.YesNo;
            categoryRequirementToUpdate.ShowValue = categoryRequirement.ShowValue;
        }

    }
}
