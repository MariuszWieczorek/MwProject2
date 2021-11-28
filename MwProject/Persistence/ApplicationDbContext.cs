using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MwProject.Core;
using MwProject.Core.Models.Domains;
using System.Reflection;

namespace MwProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Calculation> Calculations { get; set; }
        public DbSet<EstimatedSalesValue> EstimatedSalesValues { get; set; }
        public DbSet<ProjectRequirement> ProjectRequirements { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<TechnicalProperty> TechnicalProperties { get; set; }
        public DbSet<ProjectTechnicalProperty> ProjectTechnicalProperties { get; set; }
        public DbSet<CategoryTechnicalProperty> CategoryTechnicalProperties { get; set; }
        public DbSet<CategoryRequirement> CategoryRequirements { get; set; }
        public DbSet<RankingCategory> RankingCategories { get; set; }
        public DbSet<RankingElement> RankingElements { get; set; }
        public DbSet<ProjectTeamMember> ProjectTeamMembers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<TypeOfNotification> TypeOfNotifications { get; set; }
        public DbSet<ProjectStatus> ProjectStatuses { get; set; }
        public DbSet<ProjectGroup> ProjectGroups { get; set; }
        public DbSet<ProjectClient> ProjectClients { get; set; }
        public DbSet<ProjectRisk> ProjectRisks { get; set; }

 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // aby wykonały się wszystkie ustawienia wynikające z konwencji
            base.OnModelCreating(modelBuilder);
            // Fluent API
            // za pomocą mechanizmu refleksji przeszukujemy cały projekt
            // w poszukiwaniu klas implementujących interfejs IEntityTypeConfiguration.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
 

    }
}
