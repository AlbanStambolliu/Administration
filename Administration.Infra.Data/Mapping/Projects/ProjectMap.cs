using Administration.Domain.Entities.Projects;
using Administration.Infra.Data.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Administration.Infra.Data.Mapping.Projects
{
    public class ProjectMap : BaseEntityTypeConfiguration<Project>
    {
        /// <summary>
        /// Configurate
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.MapDefaults();

            builder.Property(mapping => mapping.ProjectName).IsRequired().HasMaxLength(128);
            builder.MapAuditableEntity();

            //builder.HasOne(project => project.CreatedBy)
            //   .WithMany()
            //   .HasForeignKey(project => project.Id)
            //   .OnDelete(DeleteBehavior.Restrict);



            base.Configure(builder);
        }

        protected override void PostConfigure(EntityTypeBuilder<Project> builder)
        {
            base.PostConfigure(builder);
        }
    }
}
