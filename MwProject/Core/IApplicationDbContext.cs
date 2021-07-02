using Microsoft.EntityFrameworkCore;
using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core
{
    public interface IApplicationDbContext
    {
        DbSet<Project> Projects { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Calculation> Calculations { get; set; }
        DbSet<EstimatedSalesValue> EstimatedSalesValues { get; set; }
        DbSet<Requirement> Requirements { get; set; }
        DbSet<ProjectRequirement> ProjectRequirements { get; set; }
        DbSet<ProductGroup> ProductGroups { get; set; }
        DbSet<TechnicalProperty> TechnicalProperties { get; set; }   
        DbSet<ProjectTechnicalProperty> ProjectTechnicalProperties { get; set; }
        DbSet<CategoryTechnicalProperty> CategoryTechnicalProperties { get; set; }
        DbSet<CategoryRequirement> CategoryRequirements { get; set; }
        DbSet<ApplicationUser> Users { get; set; }
        DbSet<RankingCategory>  RankingCategories { get; set; }
        DbSet<RankingElement>   RankingElements { get; set; }
        DbSet<ProjectTeamMember> ProjectTeamMembers { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<TypeOfNotification> TypeOfNotifications { get; set; }
        DbSet<ProjectStatus> ProjectStatuses { get; set; }
        DbSet<ProjectGroup> ProjectGroups { get; set; }

        int SaveChanges();
    }
}
