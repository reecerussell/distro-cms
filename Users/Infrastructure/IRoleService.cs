using System.Threading.Tasks;
using Users.Domain.Dtos;

namespace Users.Infrastructure
{
    public interface IRoleService
    {
        Task<string> CreateAsync(CreateRoleDto dto);
        Task UpdateAsync(UpdateRoleDto dto);
        Task DeleteAsync(string id);
    }
}