using Microsoft.EntityFrameworkCore;
using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MwProject.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IApplicationDbContext _context;
        public CategoryRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories
                .OrderBy(x => x.OrdinalNumber).ToList();
        }

        public Category GetCategory(int id)
        {
            var category = _context.Categories
                .Include(x => x.CategoryTechnicalProperties)
                .ThenInclude(x => x.TechnicalProperty)
                .Single(x => x.Id == id);
                    
            return category;
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
        }

        public void DeleteCategory(int id)
        {
            var categoryToDelete = _context.Categories.Single(x => x.Id == id);
            _context.Categories.Remove(categoryToDelete);
        }

        public void UpdateCategory(Category category)
        {
            var categoryToUpdate = _context.Categories.Single(x => x.Id == category.Id);
            categoryToUpdate.Name = category.Name;
            categoryToUpdate.OrdinalNumber = category.OrdinalNumber;
            categoryToUpdate.Description = category.Description;
            categoryToUpdate.DocumentSymbol = category.DocumentSymbol;
        }
    }
}
