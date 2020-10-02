using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Entity;
using System.Threading.Tasks;
using Users.Domain.Models;

namespace Users.Infrastructure.Repositories
{
    internal class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(
            DbContext context,
            ILogger<Repository<User>> logger) 
            : base(context, logger)
        {
        }

        public Task<bool> ExistsWithEmailAsync(string email)
        {
            return ExistsWithEmailAsync(email, null);
        }

        public async Task<bool> ExistsWithEmailAsync(string email, string userIdToIgnore)
        {
            return await Set.AnyAsync(x => x.Email == email && x.Id != userIdToIgnore);
        }

        public async Task<User> FindByEmailWithRolesAsync(string email)
        {
            return await Set
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
