using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Administration.Domain.Entities.Users;
using System.Threading.Tasks;
using Administration.Infra.Data.Mapping;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Administration.Domain.Entities.Projects;

namespace Administration.Infra.Data.Context
{
    public class ProjectDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, IdentityUserClaim<Guid>,
    ApplicationUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>, IDbContext
    {
        #region Fields

        private readonly IAuthenticationService _authenticationService;

        #endregion Fields

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        /// <param name="authenticationService">The authentication service.</param>
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options, IAuthenticationService authenticationService)
            : base(options) => _authenticationService = authenticationService;

        #endregion Ctor

        #region SaveChanges Overrides

        /// <inheritdoc cref="DbContext"/>
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        /// <inheritdoc cref="DbContext"/>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        /// <inheritdoc cref="DbContext"/>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc cref="DbContext"/>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        #endregion

        #region Utilities

        /// <inheritdoc cref="DbContext"/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //dynamically load all entity and query type configurations
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var typeConfigurations = assemblies.SelectMany(a => a.GetTypes()).Where(type =>
                (type.BaseType?.IsGenericType ?? false)
                && type.BaseType.GetGenericTypeDefinition() == typeof(BaseEntityTypeConfiguration<>));

            var mapList = new List<IMappingConfiguration?>();
            foreach (var typeConfiguration in typeConfigurations)
            {
                var configuration = Activator.CreateInstance(typeConfiguration) as IMappingConfiguration;
                mapList.Add(configuration);
            }
            foreach (var map in mapList.OrderBy(l => l?.Order))
            {
                map?.ApplyConfiguration(modelBuilder);
            }

            // make all decimals with a set precision
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal)
                            || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18, 2)");
            }

            // make all enums strings
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType.BaseType == typeof(Enum))
                    {
                        var type = typeof(EnumToStringConverter<>).MakeGenericType(property.ClrType);
                        var converter = Activator.CreateInstance(type, new ConverterMappingHints()) as ValueConverter;

                        property.SetValueConverter(converter);
                    }
                }
            }

            // make all ISoftDeletables hidden from query results
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.ClrType, entityBuilder =>
                {
                    //Global Filters
                    var lambdaExp = ApplyEntityFilterTo(entityType.ClrType);
                    if (lambdaExp != null)
                        entityBuilder.HasQueryFilter(lambdaExp);
                });
            }

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired()
        .OnDelete(DeleteBehavior.Restrict);

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired()
        .OnDelete(DeleteBehavior.Restrict);
            });

            //Seed: User
            modelBuilder.Entity<ApplicationUser>().HasData(
                 Seeds.SUser.Users.Administrator,
                 Seeds.SUser.Users.User);

            //Seed: Role
            modelBuilder.Entity<ApplicationRole>().HasData(
                Seeds.SUser.Roles.AdministratorRole,
                Seeds.SUser.Roles.UserRole);

            //Seed: UserRole
            modelBuilder.Entity<ApplicationUserRole>().HasData(
               Seeds.SUser.UserRoles.UserRoleAdministrator,
               Seeds.SUser.UserRoles.UserRoleUser);

            modelBuilder.Entity<ProjectUser>().HasKey(sc => new { sc.UserId, sc.ProjectId });
        }

        public DbSet<ProjectUser> ProjectUser { get; set; }

        protected virtual LambdaExpression? ApplyEntityFilterTo(Type entityClrType)
        {
            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a DbSet that can be used to query and save instances of entity.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <returns>A set for the given entity type.</returns>
        public new virtual DbSet<TEntity> Set<TEntity>() where TEntity : class => base.Set<TEntity>();
        //{
        //    return base.Set<TEntity>();
        //}

        /// <summary>
        /// Detach an entity from the context.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="entity">Entity.</param>
        public virtual void Detach<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            EntityEntry<TEntity> entityEntry = Entry(entity);
            if (entityEntry == null)
                return;

            //set the entity is not being tracked by the context
            entityEntry.State = EntityState.Detached;
        }

        #endregion
    }
}
