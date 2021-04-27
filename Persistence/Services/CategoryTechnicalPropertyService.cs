using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class CategoryTechnicalPropertyService : ICategoryTechnicalPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryTechnicalPropertyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CategoryTechnicalProperty GetCategoryTechnicalProperty(int categoryId, int id, string userId)
        {
            return _unitOfWork.CategoryTechnicalPropertyRepository.GetCategoryTechnicalProperty(categoryId, id, userId);
        }

        public CategoryTechnicalProperty NewCategoryTechnicalProperty(int categoryId, string userId)
        {
            return _unitOfWork.CategoryTechnicalPropertyRepository.NewCategoryTechnicalProperty(categoryId, userId);
        }

        public void AddCategoryTechnicalProperty(CategoryTechnicalProperty categoryTechnicalProperty, string userId)
        {
            _unitOfWork.CategoryTechnicalPropertyRepository.AddCategoryTechnicalProperty(categoryTechnicalProperty, userId);
            _unitOfWork.Complete();
        }

        public void UpdateCategoryTechnicalProperty(CategoryTechnicalProperty categoryTechnicalProperty, string userId)
        {
            _unitOfWork.CategoryTechnicalPropertyRepository.UpdateCategoryTechnicalProperty(categoryTechnicalProperty, userId);
            _unitOfWork.Complete();
        }

        public void DeleteCategoryTechnicalProperty(int categoryId, int id, string userId)
        {
            _unitOfWork.CategoryTechnicalPropertyRepository.DeleteCategoryTechnicalProperty(categoryId, id, userId);
            _unitOfWork.Complete();
        }
    }
}
