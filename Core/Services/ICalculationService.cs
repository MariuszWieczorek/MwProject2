using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Services
{
    public interface ICalculationService
    {
        Calculation GetCalculation(int projectId, int id, string userId);
        Calculation NewCalculation(int projectId, string userId);
        void AddCalculation(Calculation calculation, string userId);
        void UpdateCalculation(Calculation calculation, string userId);
        void DeleteCalculation(int projectId, int id, string userId);
    }
}
