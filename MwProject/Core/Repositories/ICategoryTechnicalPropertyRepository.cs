using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Repositories
{
    public interface ICategoryTechnicalPropertyRepository
    {
        CategoryTechnicalProperty GetCategoryTechnicalProperty(int categoryId, int id, string userId);
        CategoryTechnicalProperty NewCategoryTechnicalProperty(int categoryId, string userId);
        void AddCategoryTechnicalProperty(CategoryTechnicalProperty categoryTechnicalProperty, string userId);
        void UpdateCategoryTechnicalProperty(CategoryTechnicalProperty categoryTechnicalProperty, string userId);
        void DeleteCategoryTechnicalProperty(int categoryId, int id, string userId);
    }
}
