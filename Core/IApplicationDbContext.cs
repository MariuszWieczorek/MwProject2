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
        int SaveChanges();
    }
}
