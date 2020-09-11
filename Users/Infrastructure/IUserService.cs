using Domain.Dtos;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IUserService
    {
        Task<string> CreateAsync(CreateUserDto dto);
    }
}