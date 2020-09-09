using Domain.Dtos;
using System.Globalization;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IPageService
    {
        Task<string> CreateAsync(CreatePageDto dto, CultureInfo culture);
        Task UpdateAsync(UpdatePageDto dto, CultureInfo culture);
        Task DeleteAsync(string id);
        Task ActivateAsync(string id);
        Task DeactivateAsync(string id);
    }
}
