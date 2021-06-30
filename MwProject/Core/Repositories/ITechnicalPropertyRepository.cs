using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Repositories
{
    public interface ITechnicalPropertyRepository
    {
        IEnumerable<TechnicalProperty> GetTechnicalProperties();
        void AddTechnicalProperty(TechnicalProperty technicalProperty);
        TechnicalProperty GetTechnicalProperty(int id);
        void UpdateTechnicalProperty(TechnicalProperty technicalProperty);
        void DeleteTechnicalProperty(int id);
        void SetIsActiveToFalse(int id);
        public TechnicalProperty NewTechnicalProperty();
    }
}
