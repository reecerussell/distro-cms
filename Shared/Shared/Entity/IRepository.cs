using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Entity
{
    public interface IRepository<T> where T : Aggregate
    {
        Task<T> FindByIdAsync(string id);
        Task<T> FindWithImportsAsync(Expression<Func<T, bool>> condition,
            params Expression<Func<T, object>>[] includes);
        void Add(T item);
        void Remove(T item);
        Task SaveChangesAsync();
    }
}
