using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class TechnicalPropertyService : ITechnicalPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TechnicalPropertyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
                
        public IEnumerable<TechnicalProperty> GetTechnicalProperties()
        {
            return _unitOfWork.TechnicalPropertyRepository.GetTechnicalProperties();
        }

        public void AddTechnicalProperty(TechnicalProperty technicalProperty)
        {
            _unitOfWork.TechnicalPropertyRepository.AddTechnicalProperty(technicalProperty);
            _unitOfWork.Complete();
        }

        public TechnicalProperty GetTechnicalProperty(int id)
        {
            return _unitOfWork.TechnicalPropertyRepository.GetTechnicalProperty(id);
        }

        public void UpdateTechnicalProperty(TechnicalProperty technicalProperty)
        {
            _unitOfWork.TechnicalPropertyRepository.UpdateTechnicalProperty(technicalProperty);
            _unitOfWork.Complete();
        }

        public void DeleteTechnicalProperty(int id)
        {
            _unitOfWork.TechnicalPropertyRepository.DeleteTechnicalProperty(id);
            _unitOfWork.Complete();
        }

        public TechnicalProperty NewTechnicalProperty()
        {
            return _unitOfWork.TechnicalPropertyRepository.NewTechnicalProperty();
        }
    }
}
