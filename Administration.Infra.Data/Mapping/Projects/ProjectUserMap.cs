//using Administration.Domain.Entities.Projects;
//using Administration.Infra.Data.Extensions;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Administration.Infra.Data.Mapping.Projects
//{
//    public class ProjectUserMap : BaseEntityTypeConfiguration<ProjectUser>
//    {
//        /// <summary>
//        /// Configurate
//        /// </summary>
//        /// <param name="builder"></param>
//        public override void Configure(EntityTypeBuilder<ProjectUser> builder)
//        {
//            builder.MapDefaults();

//            builder.HasOne(x => x.User)
//              .WithMany(other => other.ProjectUsers)
//              .HasForeignKey(x => x.UserId)
//              .OnDelete(DeleteBehavior.Restrict);

//            builder.HasOne(x => x.Project)
//                .WithMany(other => other.ProjectUsers)
//                .HasForeignKey(x => x.ProjectId)
//                .OnDelete(DeleteBehavior.Restrict);

//            base.Configure(builder);
//        }

//        protected void PostConfigure(EntityTypeBuilder<ProjectUser> builder)
//        {
//            base.PostConfigure(builder);
//        }
//    }
//}
