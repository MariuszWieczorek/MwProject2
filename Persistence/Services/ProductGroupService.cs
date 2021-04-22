using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class ProductGroupService : IProductGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductGroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ProductGroup> GetProductGroups()
        {
            return _unitOfWork.ProductGroupRepository.GetProductGroups();
        }

        public void AddProductGroup(ProductGroup productGroup)
        {
            _unitOfWork.ProductGroupRepository.AddProductGroup(productGroup);
            _unitOfWork.Complete();
        }

        public ProductGroup GetProductGroup(int id)
        {
            return _unitOfWork.ProductGroupRepository.GetProductGroup(id);
        }

        public void UpdateProductGroup(ProductGroup productGroup)
        {
            _unitOfWork.ProductGroupRepository.UpdateProductGroup(productGroup);
            _unitOfWork.Complete();
        }

        public void DeleteProductGroup(int id)
        {
            _unitOfWork.ProductGroupRepository.DeleteProductGroup(id);
            _unitOfWork.Complete();
        }
    }
}
