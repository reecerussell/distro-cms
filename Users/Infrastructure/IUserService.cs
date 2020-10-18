using System.Threading.Tasks;
using Users.Domain.Dtos;

namespace Users.Infrastructure
{
    public interface IUserService
    {
        Task<(string id, string password)> CreateAsync(CreateUserDto dto);
        Task UpdateAsync(UpdateUserDto dto);
        Task ChangePasswordAsync(ChangePasswordDto dto);
        Task AddToRoleAsync(UserRoleDto dto);
        Task RemoveFromRoleAsync(UserRoleDto dto);
        Task DeleteAsync(string id);
    }
}