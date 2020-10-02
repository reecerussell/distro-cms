using Shared.Entity;
using System.Threading.Tasks;
using Users.Domain.Models;

namespace Users.Infrastructure.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> ExistsWithEmailAsync(string email);
        Task<bool> ExistsWithEmailAsync(string email, string userIdToIgnore);
        Task<User> FindByEmailWithRolesAsync(string email);
    }
}