using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MwProject.Core;
using MwProject.Core.Models.Domains;

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
        public DbSet<CategoryTechnicalProperty> CategoryTechnicalProperties { get ; set; }
        public DbSet<CategoryRequirement> CategoryRequirements { get; set; }
        public DbSet<RankingCategory> RankingCategories { get; set; }
        public DbSet<RankingElement> RankingElements { get; set; }

        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasOne<RankingCategory>(s => s.PurposeOfTheProject)
                .WithMany(g => g.Projects1)
                .HasForeignKey(s => s.PurposeOfTheProjectId);

            modelBuilder.Entity<Project>()
                .HasOne<RankingCategory>(s => s.Competitiveness)
                .WithMany(g => g.Projects2)
                .HasForeignKey(s => s.CompetitivenessId);

            modelBuilder.Entity<Project>()
                .HasOne<RankingCategory>(s => s.ViabilityOfTheProject)
                .WithMany(g => g.Projects3)
                .HasForeignKey(s => s.ViabilityOfTheProjectId);
        }
        */
        
    }
}
