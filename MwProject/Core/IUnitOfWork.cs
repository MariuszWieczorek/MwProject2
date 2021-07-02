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
        IProjectTechnicalPropertyRepository ProjectTechnicalPropertyRepository { get; set; }
        ICategoryTechnicalPropertyRepository CategoryTechnicalPropertyRepository { get; set; }
        ICategoryRequirementRepository CategoryRequirementRepository { get; set; }
        IRankingCategoryRepository RankingCategoryRepository { get; set; }
        IRankingElementRepository RankingElementRepository { get; set; }
        IUserRepository UserRepository { get; set; }
        IProjectTeamMemberRepository ProjectTeamMemberRepository { get; set; }
        INotificationRepository NotificationRepository { get; set; }
        IProjectStatusRepository ProjectStatusRepository { get; set; }
        IProjectGroupRepository ProjectGroupRepository { get; set; }


        void Complete();
    }
}
