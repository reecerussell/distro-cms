using Shared.Entity;
using System.Threading.Tasks;
using Users.Domain.Models;

namespace Users.Infrastructure
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> ExistsWithEmailAsync(string email);
        Task<bool> ExistsWithEmailAsync(string email, string userIdToIgnore);
    }
}