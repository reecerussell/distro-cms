using Dictionary.Domain.Dtos;
using System.Threading.Tasks;

namespace Dictionary.Infrastructure
{
    public interface ISupportedCultureService
    {
        Task<string> CreateAsync(CreateSupportedCultureDto dto);
        Task SetAsDefaultAsync(string id);
        Task UpdateAsync(UpdateSupportedCultureDto dto);
        Task DeleteAsync(string id);
    }
}
