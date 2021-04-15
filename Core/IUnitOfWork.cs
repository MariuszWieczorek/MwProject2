using MwProject.Core.Repositories;

namespace MwProject.Core
{
    public interface IUnitOfWork
    {
        IProjectRepository Project { get; set; }
        ICategoryRepository Category { get; set; }
        ICalculationRepository Calculation { get; set; }
        void Complete();
    }
}
