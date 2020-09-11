using Domain.Models;
using Shared.Entity;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> ExistsWithEmailAsync(string email);
        Task<bool> ExistsWithEmailAsync(string email, string userIdToIgnore);
    }
}