using Administration.Domain.Entities.Projects;
using Administration.Infra.Data.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Administration.Infra.Data.Mapping.Projects
{
    public class ProjectTaskMap : BaseEntityTypeConfiguration<ProjectTask>
    {
        /// <summary>
        /// Configurate
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.MapDefaults();

            builder.Property(mapping => mapping.TaskName).IsRequired().HasMaxLength(128);
            builder.MapAuditableEntity();

            base.Configure(builder);
        }

        protected override void PostConfigure(EntityTypeBuilder<ProjectTask> builder)
        {
            base.PostConfigure(builder);
        }
    }
}
