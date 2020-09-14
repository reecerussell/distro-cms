using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Entity
{
    public class Repository<T> : IRepository<T> where T : Aggregate
    {
        private readonly DbContext _context;
        private DbSet<T> _set;

        protected DbSet<T> Set => _set ??= _context.Set<T>();
        protected readonly ILogger<Repository<T>> Logger;

        public Repository(
            DbContext context,
            ILogger<Repository<T>> logger)
        {
            _context = context;
            Logger = logger;
        }

        public virtual async Task<T> FindByIdAsync(string id)
        {
            return await Set.FindAsync(id);
        }

        public virtual async Task<T> FindWithImportsAsync(Expression<Func<T, bool>> condition, params Expression<Func<T, object>>[] includes)
        {
            var query = includes.Aggregate<Expression<Func<T, object>>, IQueryable<T>>(Set, 
                (current, include) => current.Include(include));

            return await query.FirstOrDefaultAsync(condition);
        }

        public virtual void Add(T item)
        {
            Logger.LogDebug("Adding item with id: {0}", item.Id);

            Set.Add(item);
        }

        public virtual void Remove(T item)
        {
            Logger.LogDebug("Removing item with id: {0}", item.Id);

            Set.Remove(item);
        }

        public virtual async Task SaveChangesAsync()
        {
            try
            {
                Logger.LogDebug("Saving changes to the database.");

                // Set the DateUpdated field for updated entities.
                var entities = _context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Modified);
                foreach (var entity in entities)
                {
                    var obj = (Entity)entity.Entity;
                    obj.DateUpdated = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                Logger.LogDebug("Successfully saved changes.");
            }
            catch (DbUpdateConcurrencyException e)
            {
                Logger.LogError(e, "A database concurrency error occured.");

                throw;
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(e, "A database update error occured.");

                throw;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An unknown error occured while saving to the database.");

                throw;
            }
        }
    }
}
