using Dictionary.Domain.Dtos;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Dictionary.Infrastructure
{
    public interface ISupportedCultureProvider
    {
        Task<IReadOnlyList<SupportedCultureDropdownItemDto>> GetDropdownItemsAsync(CultureInfo culture);
    }
}