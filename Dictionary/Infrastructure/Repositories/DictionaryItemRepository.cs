using Dictionary.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Entity;
using System.Threading.Tasks;

namespace Dictionary.Infrastructure.Repositories
{
    internal class DictionaryItemRepository : Repository<DictionaryItem>, IDictionaryItemRepository
    {
        public DictionaryItemRepository(
            DbContext context, 
            ILogger<Repository<DictionaryItem>> logger) 
            : base(context, logger)
        {
        }

        public Task<bool> ExistsAsync(string key, SupportedCulture culture)
        {
            return Set
                .Include(x => x.Culture)
                .AnyAsync(x => x.Key == key && x.Culture == culture);
        }
    }
}