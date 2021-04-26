using MwProject.Core.Repositories;

namespace MwProject.Core
{
    public interface IUnitOfWork
    {
        IProjectRepository Project { get; set; }
        ICategoryRepository Category { get; set; }
        ICalculationRepository Calculation { get; set; }
        IEstimatedSalesValueRepository EstimatedSalesValue { get; set; }
        IRequirementRepository Requirement { get; set; }
        IProjectRequirementRepository ProjectRequirement { get; set; }
        IProductGroupRepository ProductGroupRepository { get; set; }
        ITechnicalPropertyRepository TechnicalPropertyRepository { get; set; }
        void Complete();
    }
}
