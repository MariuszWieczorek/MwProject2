using MwProject.Core;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        // readonly przy polu oznacza, że jego wartość
        // możemy zmienić tylko w konstruktorze
        private readonly IApplicationDbContext _context;
        public UnitOfWork(IApplicationDbContext context,
                            IProjectRepository project,
                            ICategoryRepository category,
                            ICalculationRepository calculationRepository,
                            IEstimatedSalesValueRepository estimatedSalesValueRepository,
                            IRequirementRepository requirementRepository,
                            IProjectRequirementRepository projectRequirementRepository,
                            IProductGroupRepository productGroupRepository,
                            ITechnicalPropertyRepository technicalPropertyRepository,
                            IProjectTechnicalPropertyRepository projectTechnicalPropertyRepository,
                            ICategoryTechnicalPropertyRepository categoryTechnicalPropertyRepository,
                            ICategoryRequirementRepository categoryRequirementRepository,
                            IUserRepository userRepository,
                            IRankingCategoryRepository rankingCategoryRepository,
                            IRankingElementRepository rankingElementRepository,
                            IProjectTeamMemberRepository projectTeamMemberRepository,
                            INotificationRepository notificationRepository,
                            IProjectStatusRepository projectStatusRepository,
                            IProjectGroupRepository projectGroupRepository,
                            IProjectClientRepository projectClientRepository
                            )
        {
            _context = context;
            Project = project;                      // new AdvertRepository(context);
            Category = category;                    // new CategoryRepository(context);
            Calculation = calculationRepository;
            EstimatedSalesValue = estimatedSalesValueRepository;
            Requirement = requirementRepository;
            ProjectRequirement = projectRequirementRepository;
            ProductGroupRepository = productGroupRepository;
            TechnicalPropertyRepository = technicalPropertyRepository;
            ProjectTechnicalPropertyRepository = projectTechnicalPropertyRepository;
            CategoryTechnicalPropertyRepository = categoryTechnicalPropertyRepository;
            CategoryRequirementRepository = categoryRequirementRepository;
            UserRepository = userRepository;
            RankingCategoryRepository = rankingCategoryRepository;
            RankingElementRepository = rankingElementRepository;
            ProjectTeamMemberRepository = projectTeamMemberRepository;
            NotificationRepository = notificationRepository;
            ProjectStatusRepository = projectStatusRepository;
            ProjectGroupRepository = projectGroupRepository;
            ProjectClientRepository = projectClientRepository;
        }

        // obiekty repozytoryjne 
        public IProjectRepository Project { get; set; }
        public ICategoryRepository Category { get; set; }
        public ICalculationRepository Calculation { get; set; }
        public IEstimatedSalesValueRepository EstimatedSalesValue { get; set; }
        public IRequirementRepository Requirement { get; set; }
        public IProjectRequirementRepository ProjectRequirement { get; set; }
        public IProductGroupRepository ProductGroupRepository { get; set; }
        public ITechnicalPropertyRepository TechnicalPropertyRepository { get; set; }
        public IProjectTechnicalPropertyRepository ProjectTechnicalPropertyRepository { get; set; }
        public ICategoryTechnicalPropertyRepository CategoryTechnicalPropertyRepository { get; set; }
        public ICategoryRequirementRepository CategoryRequirementRepository { get; set; }
        public IRankingCategoryRepository RankingCategoryRepository { get; set; }
        public IRankingElementRepository RankingElementRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IProjectTeamMemberRepository ProjectTeamMemberRepository { get; set; }
        public INotificationRepository NotificationRepository { get; set; }
        public IProjectStatusRepository ProjectStatusRepository { get; set; }
        public IProjectGroupRepository ProjectGroupRepository { get; set; }
        public IProjectClientRepository ProjectClientRepository { get; set; }

        // na koniec metoda zapisująca zmiany
        public void Complete()
        {
            _context.SaveChanges();
        }

    }
}
