using Dictionary.Domain.Models;
using Shared.Entity;
using System.Globalization;
using System.Threading.Tasks;

namespace Dictionary.Infrastructure.Repositories
{
    public interface IDictionaryItemRepository : IRepository<DictionaryItem>
    {
        Task<bool> ExistsAsync(string key, CultureInfo culture);
    }
}