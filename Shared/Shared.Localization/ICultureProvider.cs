using Shared.Localization.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Localization
{
    public interface ICultureProvider
    {
        Task<IReadOnlyList<CultureDto>> GetCultures();
    }
}