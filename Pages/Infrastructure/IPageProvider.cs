using Domain.Dtos;
using System.Globalization;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IPageProvider
    {
        Task<PageDto> GetPageAsync(string id, CultureInfo culture);
    }
}