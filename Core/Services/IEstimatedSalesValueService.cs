using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Services
{
    public interface IEstimatedSalesValueService
    {
        EstimatedSalesValue GetEstimatedSalesValue(int projectId, int id, string userId);
        EstimatedSalesValue NewEstimatedSalesValue(int projectId, string userId);
        void AddEstimatedSalesValue(EstimatedSalesValue estimatedSalesValue, string userId);
        void UpdateEstimatedSalesValue(EstimatedSalesValue estimatedSalesValue, string userId);
        void DeleteEstimatedSalesValue(int projectId, int id, string userId);
    }
}
