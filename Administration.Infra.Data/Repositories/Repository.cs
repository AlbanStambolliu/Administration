using Administration.Infra.Data.Context;
using Administration.Infra.Data.Repositories;
using Administration.Infra.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Administration.Infra.Data.Repositories
{
    /// <summary>.
    /// Repository
    /// </summary>
    /// <typeparam name="TEntity">Entity.</typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Fields

        private readonly IDbContext _context;
        private DbSet<TEntity> _entities;
        private readonly ILogger<Repository<TEntity>> _logger;

        #endregion Fields

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="logger">The logger.</param>
        public Repository(IDbContext context, ILogger<Repository<TEntity>> logger)
        {
            _context = context;
            _logger = logger;
            _entities = _context.Set<TEntity>();
        }

        #endregion Ctor

        #region Utilities

        /// <summary>
        /// Rollback of entity changes and return full error message.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <returns>Error message.</returns>
        protected string GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception)
        {
            //rollback entity changes
            if (_context is DbContext dbContext)
            {
                List<EntityEntry> entries = dbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

                entries.ForEach(entry => entry.State = EntityState.Unchanged);
            }

            _context.SaveChanges();
            return exception.ToString();
        }

        #endregion Utilities

        #region Methods

        #region Insert

        /// <summary>
        /// Insert entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Add(entity);
                if (AutoSaveChanges) _context.SaveChanges();

                LogOperation("Insert", entity);
            }
            catch (DbUpdateException exception)
            {
                LogOperationError(exception);

                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Asynchronously insert entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        public virtual async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                await Entities.AddAsync(entity, cancellationToken);
                if (AutoSaveChanges) await _context.SaveChangesAsync(cancellationToken);

                LogOperation("Insert", entity);
            }
            catch (DbUpdateException exception)
            {
                LogOperationError(exception);

                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Insert entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.AddRange(entities);
                if (AutoSaveChanges) _context.SaveChanges();

                LogOperation("Insert", entities);
                _logger.LogTrace("Finished insert operation for {entities}", entities);
            }
            catch (DbUpdateException exception)
            {
                LogOperationError(exception);

                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Asynchronously insert entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        public virtual async Task InsertAsync(IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                await Entities.AddRangeAsync(entities, cancellationToken);
                if (AutoSaveChanges) await _context.SaveChangesAsync(cancellationToken);

                LogOperation("Insert", entities);
            }
            catch (DbUpdateException exception)
            {
                LogOperationError(exception);

                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        #endregion Insert

        #region Edit

        /// <summary>
        /// Edit entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public virtual void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Update(entity);
                if (AutoSaveChanges) _context.SaveChanges();

                LogOperation("Update", entity);
            }
            catch (DbUpdateException exception)
            {
                LogOperationError(exception);

                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Asynchronously update entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Update(entity);
                if (AutoSaveChanges) await _context.SaveChangesAsync(cancellationToken);

                LogOperation("Update", entity);
            }
            catch (DbUpdateException exception)
            {
                LogOperationError(exception);

                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Edit entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        public virtual void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.UpdateRange(entities);
                if (AutoSaveChanges) _context.SaveChanges();

                LogOperation("Update", entities);
            }
            catch (DbUpdateException exception)
            {
                LogOperationError(exception);

                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Asynchronously update entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        public virtual async Task UpdateAsync(IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.UpdateRange(entities);
                if (AutoSaveChanges) await _context.SaveChangesAsync(cancellationToken);

                LogOperation("Update", entities);
            }
            catch (DbUpdateException exception)
            {
                LogOperationError(exception);

                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        #endregion Edit

        #region Delete

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Remove(entity);
                if (AutoSaveChanges) _context.SaveChanges();

                LogOperation("Delete", entity);
            }
            catch (DbUpdateException exception)
            {
                LogOperationError(exception);

                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Asynchronously delete entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Remove(entity);
                if (AutoSaveChanges) await _context.SaveChangesAsync(cancellationToken);

                LogOperation("Delete", entity);
            }
            catch (DbUpdateException exception)
            {
                LogOperationError(exception);

                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Delete entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.RemoveRange(entities);
                if (AutoSaveChanges) _context.SaveChanges();

                LogOperation("Delete", entities);
            }
            catch (DbUpdateException exception)
            {
                LogOperationError(exception);

                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Asynchronously delete entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        public virtual async Task DeleteAsync(IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.RemoveRange(entities);
                if (AutoSaveChanges) await _context.SaveChangesAsync(cancellationToken);

                LogOperation("Delete", entities);
            }
            catch (DbUpdateException exception)
            {
                LogOperationError(exception);

                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        #endregion Delete

        #region GetAsync
        /// <summary>
        /// Asynchronously get entity by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>Entity.</returns>
        public virtual async Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await Entities.FindAsync(new[] { id }, cancellationToken);
        }
        #endregion

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether IDbContext.SaveChanges() should be
        /// called after every operation.
        /// </summary>
        public bool AutoSaveChanges { get; set; } = true;

        /// <summary>
        /// Gets a table.
        /// </summary>
        public virtual IQueryable<TEntity> Table
        {
            get
            {
                _logger.LogTrace("Getting a {Table} table.", nameof(TEntity));
                return Entities;
            }
        }

        /// <summary>
        /// Gets a table with no-tracking enabled. Should be used only for read only operations.
        /// </summary>
        public virtual IQueryable<TEntity> TableNoTracking
        {
            get
            {
                _logger.LogTrace("Getting a readonly {Table} table.", nameof(TEntity));
                return Entities.AsNoTracking();
            }
        }

        /// <summary>
        /// Gets an entity set.
        /// </summary>
        protected virtual DbSet<TEntity> Entities => _entities ??= _context.Set<TEntity>();

        #endregion Properties

        #region Log

        /// <summary>
        /// Add log for entity.
        /// </summary>
        /// <param name="type">Entity type.</param>
        /// <param name="entity">Entity.</param>
        private void LogOperation(string type, TEntity entity) =>
            _logger.LogTrace("Finished {OperationType} operation on table {Table}", type, nameof(TEntity));

        /// <summary>
        /// Add log for entities
        /// </summary>
        /// <param name="type">Entity type.</param>
        /// <param name="entities">Entities.</param>
        private void LogOperation(string type, IEnumerable<TEntity> entities) =>
            _logger.LogTrace("Finished {OperationType} operation on table {Table} for {Count} objects.", type,
                nameof(TEntity), entities.Count());

        /// <summary>
        /// Add log for exceptions
        /// </summary>
        /// <param name="exception">Exception.</param>
        private void LogOperationError(Exception exception) =>
            _logger.LogError("A database error occurred, please check the exception for more details." +
                             " Exception is {@Exception}", exception);

        #endregion Log
    }
}
