using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Entity;
using System.Threading.Tasks;
using Users.Domain.Models;

namespace Users.Infrastructure.Repositories
{
    internal class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(
            DbContext context, 
            ILogger<Repository<Role>> logger) 
            : base(context, logger)
        {
        }

        public Task<bool> ExistsWithNameAsync(string name)
        {
            return ExistsWithNameAsync(name, null);
        }

        public async Task<bool> ExistsWithNameAsync(string name, string idToIgnore)
        {
            return await Set.AnyAsync(x => x.Name == name && x.Id != idToIgnore);
        }
    }
}
