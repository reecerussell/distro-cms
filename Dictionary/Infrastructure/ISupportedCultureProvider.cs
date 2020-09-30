using Dictionary.Domain.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dictionary.Infrastructure
{
    public interface ISupportedCultureProvider
    {
        Task<SupportedCultureDto> GetAsync(string id);
        Task<IReadOnlyList<SupportedCultureListItemDto>> GetListAsync(string term = null);
        Task<IReadOnlyList<SupportedCultureDropdownItemDto>> GetDropdownItemsAsync();
        Task<IReadOnlyList<SupportedCultureDropdownItemDto>> GetAvailableDropdownItemsAsync();
    }
}