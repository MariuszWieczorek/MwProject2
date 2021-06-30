using Microsoft.EntityFrameworkCore;
using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MwProject.Persistence.Repositories
{
    public class TechnicalPropertyRepository : ITechnicalPropertyRepository
    {
        private readonly IApplicationDbContext _context;
        public TechnicalPropertyRepository(IApplicationDbContext context)
        {
            _context = context;
        }

         
        public IEnumerable<TechnicalProperty> GetTechnicalProperties()
        {
            var technicalProperities = _context.TechnicalProperties
                        .Where(x => x.IsActive)
                        .OrderBy(x => x.Name)
                        .ToList();
                            
            return technicalProperities;
        }

        public void AddTechnicalProperty(TechnicalProperty technicalProperty)
        {
            technicalProperty.IsActive = true;
            _context.TechnicalProperties.Add(technicalProperty);
            //var sql = string.Format(@"SELECT IDENT_CURRENT ('{0}') AS Current_Identity", "TechnicalProperties");
            //var id = _context.TechnicalProperties.FromSqlRaw(sql).First();
            int id = _context.TechnicalProperties.Max(x => x.Id);
        }

        public TechnicalProperty GetTechnicalProperty(int id)
        {
           var technicalProperty = _context.TechnicalProperties.Single(x => x.Id == id);
           return technicalProperty;
        }

        public void UpdateTechnicalProperty(TechnicalProperty technicalProperty)
        {
            var technicalPropertyToUpdate = _context.TechnicalProperties.Single(x => x.Id == technicalProperty.Id);
            technicalPropertyToUpdate.Name = technicalProperty.Name;
            technicalPropertyToUpdate.OrdinalNumber = technicalProperty.OrdinalNumber;
        }

        public void DeleteTechnicalProperty(int id)
        {
            var technicalPropertyToDelete = _context.TechnicalProperties.Single(x => x.Id == id);
            _context.TechnicalProperties.Remove(technicalPropertyToDelete);
        }

        public TechnicalProperty NewTechnicalProperty()
        {
            return new TechnicalProperty()
            {
                OrdinalNumber = _context.TechnicalProperties.Max(x => x.OrdinalNumber) + 1
            };
        }

        public void SetIsActiveToFalse(int id)
        {
            var technicalPropertyToUpdate = _context.TechnicalProperties.Single(x => x.Id == id);
            technicalPropertyToUpdate.IsActive = false;
        }
    }
}
