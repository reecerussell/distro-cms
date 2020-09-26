using Dictionary.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Entity;
using System.Threading.Tasks;

namespace Dictionary.Infrastructure.Repositories
{
    internal class SupportedCultureRepository : Repository<SupportedCulture>, ISupportedCultureRepository
    {
        public SupportedCultureRepository(
            DictionaryContext context, 
            ILogger<Repository<SupportedCulture>> logger) 
            : base(context, logger)
        {
        }

        public async Task<SupportedCulture> FindByNameAsync(string name)
        {
            return await Set.SingleOrDefaultAsync(x => x.Name == name);
        }
    }
}