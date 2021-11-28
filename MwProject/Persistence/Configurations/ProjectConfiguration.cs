
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MwProject.Core.Models.Domains;


namespace MwProject.Persistence.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder
                .Property(x => x.Title)
                .IsRequired();

            // relacja z ApplicationUser
           builder.HasOne<ApplicationUser>(p => p.User)
          .WithMany(c => c.ProjectAuthors)
          .HasForeignKey(p => p.UserId)
          .OnDelete(DeleteBehavior.Restrict);

           builder.HasOne<ApplicationUser>(p => p.ProjectManager)
          .WithMany(c => c.ProjectManagers)
          .HasForeignKey(p => p.ProjectManagerId)
          .OnDelete(DeleteBehavior.Restrict);
                 
      
        }
    }
}
