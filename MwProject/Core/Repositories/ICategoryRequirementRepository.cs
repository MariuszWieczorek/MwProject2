using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Repositories
{
    public interface ICategoryRequirementRepository
    {
        CategoryRequirement GetCategoryRequirement(int categoryId, int id, string userId);
        CategoryRequirement NewCategoryRequirement(int categoryId, string userId, int type);
        void AddCategoryRequirement(CategoryRequirement categoryRequirement, string userId);
        void UpdateCategoryRequirement(CategoryRequirement categoryRequirement, string userId);
        void DeleteCategoryRequirement(int categoryId, int id, string userId);
    }
}
