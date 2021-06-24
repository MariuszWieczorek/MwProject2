using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Persistence.Repositories
{
    public class CategoryTechnicalPropertyRepository : ICategoryTechnicalPropertyRepository
    {

        private readonly IApplicationDbContext _context;
        public CategoryTechnicalPropertyRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public void AddCategoryTechnicalProperty(CategoryTechnicalProperty categoryTechnicalProperty, string userId)
        {
            _context.CategoryTechnicalProperties.Add(categoryTechnicalProperty);
        }


        public void DeleteCategoryTechnicalProperty(int categoryId, int id, string userId)
        {
            var categoryTechnicalPropertyToDelete = _context.CategoryTechnicalProperties.Single(x => x.CategoryId == categoryId);
            _context.CategoryTechnicalProperties.Remove(categoryTechnicalPropertyToDelete);
        }

        public CategoryTechnicalProperty GetCategoryTechnicalProperty(int categoryId, int id, string userId)
        {
            var categoryTechnicalProperty = _context.CategoryTechnicalProperties.Single(x => x.CategoryId == categoryId && x.Id == id);
            return categoryTechnicalProperty;
        }

        public CategoryTechnicalProperty NewCategoryTechnicalProperty(int categoryId, string userId)
        {
            int ordinalNumber = 1;
            if(_context.CategoryTechnicalProperties.Where(x => x.CategoryId == categoryId).Any())
            {
                ordinalNumber = _context.CategoryTechnicalProperties.Where(x => x.CategoryId == categoryId).Max(x => x.OrdinalNumber) + 1;
            }

            return new CategoryTechnicalProperty
            {
                CategoryId = categoryId,
                OrdinalNumber = ordinalNumber,
                Exist = true
            };
        }

        public void UpdateCategoryTechnicalProperty(CategoryTechnicalProperty categoryTechnicalProperty, string userId)
        {
            var categoryTechnicalPropertyToUpdate = _context.CategoryTechnicalProperties
                .Single(x => x.CategoryId == categoryTechnicalProperty.CategoryId && x.Id == categoryTechnicalProperty.Id);

            categoryTechnicalPropertyToUpdate.TechnicalPropertyId = categoryTechnicalProperty.TechnicalPropertyId;
            categoryTechnicalPropertyToUpdate.Exist = categoryTechnicalProperty.Exist;
            categoryTechnicalPropertyToUpdate.Value = categoryTechnicalProperty.Value;
            categoryTechnicalPropertyToUpdate.Comment = categoryTechnicalProperty.Comment;
            categoryTechnicalPropertyToUpdate.OrdinalNumber = categoryTechnicalProperty.OrdinalNumber;
            categoryTechnicalPropertyToUpdate.YesNo = categoryTechnicalProperty.YesNo;
            categoryTechnicalPropertyToUpdate.ShowValue = categoryTechnicalProperty.ShowValue;
        }

    }
}
