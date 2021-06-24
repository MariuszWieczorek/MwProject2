using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CalculationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Calculation GetCalculation(int projectId, int id, string userId)
        {
            return _unitOfWork.Calculation.GetCalculation(projectId, id, userId);
        }

        public Calculation NewCalculation(int projectId, string userId)
        {
            return _unitOfWork.Calculation.NewCalculation(projectId, userId);
        }

        public void AddCalculation(Calculation calculation, string userId)
        {
            _unitOfWork.Calculation.AddCalculation(calculation, userId);
            _unitOfWork.Complete();
        }

        public void UpdateCalculation(Calculation calculation, string userId)
        {
            _unitOfWork.Calculation.UpdateCalculation(calculation, userId);
            _unitOfWork.Complete();
        }

        public void DeleteCalculation(int projectId, int id, string userId)
        {
            _unitOfWork.Calculation.DeleteCalculation(projectId, id, userId);
            _unitOfWork.Complete();
        }
    }
}
