using Shared.Entity;
using System.Threading.Tasks;
using Users.Domain.Models;

namespace Users.Infrastructure.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<bool> ExistsWithNameAsync(string name);
        Task<bool> ExistsWithNameAsync(string name, string idToIgnore);
    }
}