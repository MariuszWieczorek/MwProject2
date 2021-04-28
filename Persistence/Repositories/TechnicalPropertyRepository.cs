﻿using MwProject.Core;
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
                        .OrderBy(x => x.OrdinalNumber)
                        .ThenBy(x => x.Name)
                        .ToList();
                            
            return technicalProperities;
        }

        public void AddTechnicalProperty(TechnicalProperty technicalProperty)
        {
            _context.TechnicalProperties.Add(technicalProperty);
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
    }
}
