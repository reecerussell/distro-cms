using System.Collections.Generic;
using System.Threading.Tasks;
using Users.Domain.Dtos;

namespace Users.Infrastructure
{
    public interface IRoleProvider
    {
        Task<IReadOnlyList<RoleListItemDto>> GetListAsync(string term);
        Task<RoleDto> GetAsync(string id);
    }
}