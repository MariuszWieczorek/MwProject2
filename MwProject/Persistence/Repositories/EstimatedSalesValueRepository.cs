using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Persistence.Repositories
{
    public class EstimatedSalesValueRepository : IEstimatedSalesValueRepository
    {

        private readonly IApplicationDbContext _context;
        public EstimatedSalesValueRepository(IApplicationDbContext context)
        {
            _context = context;
        }
   
        public EstimatedSalesValue GetEstimatedSalesValue(int projectId, int id, string userId)
        {
            var estimatedSalesValue = _context.EstimatedSalesValues.Single(x => x.ProjectId == projectId && x.Id == id);
            return estimatedSalesValue;
        }

        public EstimatedSalesValue NewEstimatedSalesValue(int projectId, string userId)
        {
            return new EstimatedSalesValue
            {
                ProjectId = projectId,
            };
        }

        public void AddEstimatedSalesValue(EstimatedSalesValue estimatedSalesValue, string userId)
        {
            _context.EstimatedSalesValues.Add(estimatedSalesValue);
        }

        public void UpdateEstimatedSalesValue(EstimatedSalesValue estimatedSalesValue, string userId)
        {
            var salesToUpdate = _context.EstimatedSalesValues.Single(x => x.Id == estimatedSalesValue.Id && x.ProjectId == estimatedSalesValue.ProjectId);
            salesToUpdate.Year = estimatedSalesValue.Year;
            salesToUpdate.Qty = estimatedSalesValue.Qty;
            salesToUpdate.Price = estimatedSalesValue.Price;
        }

        public void DeleteEstimatedSalesValue(int projectId, int id, string userId)
        {
            var salesToDelete = _context.EstimatedSalesValues.Single(x => x.Id == id && x.ProjectId == projectId);
            _context.EstimatedSalesValues.Remove(salesToDelete);
        }
    }
}
