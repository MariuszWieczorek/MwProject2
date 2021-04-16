using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Persistence.Repositories
{
    public class CalculationRepository : ICalculationRepository
    {

        private readonly IApplicationDbContext _context;
        public CalculationRepository(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public Calculation GetCalculation(int projectId, int id, string userId)
        {
            var calculation = _context.Calculations.Single(x => x.ProjectId == projectId && x.Id == id);
            return calculation;
        }

        public Calculation NewCalculation(int projectId, string userId)
        {
            return new Calculation
            { 
                ProjectId = projectId,
            };
        }

        public void AddCalculation(Calculation calculation, string userId)
        {
            var tkw = calculation.MaterialCosts + calculation.LabourCosts + calculation.PackingCosts + calculation.Markup;
            var ckw = tkw * (1 + calculation.GeneralCostsInPercent * 0.01M);
            calculation.Tkw = tkw;
            calculation.Ckw = ckw;
            _context.Calculations.Add(calculation);
        }

        public void UpdateCalculation(Calculation calculation, string userId)
        {
            var calculationToUpdate = _context.Calculations.Single(x => x.Id == calculation.Id);
            var tkw = calculation.MaterialCosts + calculation.LabourCosts + calculation.PackingCosts + calculation.Markup;
            var ckw = tkw * (1 + calculation.GeneralCostsInPercent * 0.01M);
            calculationToUpdate.MaterialCosts = calculation.MaterialCosts;
            calculationToUpdate.LabourCosts = calculation.LabourCosts;
            calculationToUpdate.PackingCosts = calculation.PackingCosts;
            calculationToUpdate.Markup = calculation.Markup;
            calculationToUpdate.GeneralCostsInPercent = calculation.GeneralCostsInPercent;
            calculationToUpdate.Tkw = tkw;
            calculationToUpdate.Ckw = ckw;
        }

        public void DeleteCalculation(int projectId, int id, string userId)
        {
            var calculationToDelete = _context.Calculations.Single(x => x.Id == id && x.ProjectId == projectId);
            _context.Calculations.Remove(calculationToDelete);
        }

    }
}
