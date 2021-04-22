using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MwProject.Persistence.Repositories
{
    public class ProductGroupRepository : IProductGroupRepository
    {
        private readonly IApplicationDbContext _context;
        public ProductGroupRepository(IApplicationDbContext context)
        {
            _context = context;
        }

                
        public IEnumerable<ProductGroup> GetProductGroups()
        {
            return _context.ProductGroups
            .OrderBy(x => x.OrdinalNumber).ToList();
        }

        public void AddProductGroup(ProductGroup productGroup)
        {
            _context.ProductGroups.Add(productGroup);
        }

        public ProductGroup GetProductGroup(int id)
        {
            return _context.ProductGroups.Single(x => x.Id == id);
        }

        public void UpdateProductGroup(ProductGroup productGroup)
        {
            var productGroupToUpdate = _context.ProductGroups.Single(x => x.Id == productGroup.Id);
            productGroupToUpdate.Name = productGroup.Name;
            productGroupToUpdate.OrdinalNumber = productGroup.OrdinalNumber;
        }

        public void DeleteProductGroup(int id)
        {
            var productGroupToRemove = _context.ProductGroups.Single(x => x.Id == id);
            _context.ProductGroups.Remove(productGroupToRemove);
        }
    }
}
