using Dictionary.Domain.Dtos;
using System.Globalization;
using System.Threading.Tasks;

namespace Dictionary.Infrastructure
{
    public interface IDictionaryService
    {
        Task<string> CreateAsync(CreateDictionaryItem dto, CultureInfo culture);
        Task UpdateAsync(UpdateDictionaryItem dto);
        Task DeleteAsync(string id);
    }
}