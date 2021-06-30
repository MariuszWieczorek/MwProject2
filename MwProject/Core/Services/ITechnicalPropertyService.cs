using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Services
{
    public interface ITechnicalPropertyService
    {
        IEnumerable<TechnicalProperty> GetTechnicalProperties();
        void AddTechnicalProperty(TechnicalProperty technicalProperty);
        TechnicalProperty GetTechnicalProperty(int id);
        void UpdateTechnicalProperty(TechnicalProperty technicalProperty);
        void SetIsActiveToFalse(int id);
        void DeleteTechnicalProperty(int id);
        TechnicalProperty NewTechnicalProperty();
    }
}
