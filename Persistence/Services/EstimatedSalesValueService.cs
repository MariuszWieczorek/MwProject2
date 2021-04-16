using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class EstimatedSalesValueService : IEstimatedSalesValueService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EstimatedSalesValueService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public EstimatedSalesValue GetEstimatedSalesValue(int projectId, int id, string userId)
        {
            return _unitOfWork.EstimatedSalesValue.GetEstimatedSalesValue(projectId, id, userId);
        }

        public EstimatedSalesValue NewEstimatedSalesValue(int projectId, string userId)
        {
           return _unitOfWork.EstimatedSalesValue.NewEstimatedSalesValue(projectId, userId);
        }

        public void AddEstimatedSalesValue(EstimatedSalesValue estimatedSalesValue, string userId)
        {
            _unitOfWork.EstimatedSalesValue.AddEstimatedSalesValue(estimatedSalesValue, userId);
            _unitOfWork.Complete();
        }

        public void UpdateEstimatedSalesValue(EstimatedSalesValue estimatedSalesValue, string userId)
        {
            _unitOfWork.EstimatedSalesValue.UpdateEstimatedSalesValue(estimatedSalesValue, userId);
            _unitOfWork.Complete();
        }

        public void DeleteEstimatedSalesValue(int projectId, int id, string userId)
        {
            _unitOfWork.EstimatedSalesValue.DeleteEstimatedSalesValue(projectId, id, userId);
            _unitOfWork.Complete();
        }
    }
}
