using Dictionary.Domain.Dtos;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Dictionary.Infrastructure
{
    public interface IDictionaryItemProvider
    {
        Task<DictionaryItemDto> GetAsync(string id);
        Task<IReadOnlyList<DictionaryListItemDto>> GetListAsync(CultureInfo culture);
    }
}