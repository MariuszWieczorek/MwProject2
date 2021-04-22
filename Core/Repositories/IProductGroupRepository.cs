using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Repositories
{
    public interface IProductGroupRepository
    {
        IEnumerable<ProductGroup> GetProductGroups();
        void AddProductGroup(ProductGroup productGroup);
        ProductGroup GetProductGroup(int id);
        void UpdateProductGroup(ProductGroup productGroup);
        void DeleteProductGroup(int id);
    }
}
