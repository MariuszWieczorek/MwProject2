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
    }
}
