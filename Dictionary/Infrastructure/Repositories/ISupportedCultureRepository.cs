using Dictionary.Domain.Models;
using Shared.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dictionary.Infrastructure.Repositories
{
    public interface ISupportedCultureRepository : IRepository<SupportedCulture>
    {
        Task<SupportedCulture> FindByNameAsync(string name);
        Task<bool> ExistsWithNameAsync(string name);
        Task<IReadOnlyList<SupportedCulture>> GetDefaultCulturesAsync();
    }
}