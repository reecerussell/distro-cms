using Pages.Domain.Dtos;
using System.Globalization;
using System.Threading.Tasks;

namespace Pages.Infrastructure
{
    public interface IPageProvider
    {
        Task<PageDto> GetPageAsync(string id, CultureInfo culture);
    }
}