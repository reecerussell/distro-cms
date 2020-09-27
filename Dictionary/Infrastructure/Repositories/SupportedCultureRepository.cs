using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> ExistsWithNameAsync(string name)
        {
            return await Set.AnyAsync(x => x.Name == name);
        }

        /// <summary>
        /// This should only ever return a single <see cref="SupportedCulture"/>, but the
        /// datamodel allows for multiple.
        /// </summary>
        /// <returns>A list of <see cref="SupportedCulture"/> which are marked as default.</returns>
        public async Task<IReadOnlyList<SupportedCulture>> GetDefaultCulturesAsync()
        {
            return await Set.Where(x => x.IsDefault).ToListAsync();
        }
    }
}