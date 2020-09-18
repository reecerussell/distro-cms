using Dapper;
using Dictionary.Domain.Dtos;
using Microsoft.Extensions.Logging;
using Shared;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary.Infrastructure.Providers
{
    internal class DictionaryItemProvider : IDictionaryItemProvider
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly ILogger<DictionaryItemProvider> _logger;

        public DictionaryItemProvider(
            IConnectionStringProvider connectionStringProvider,
            ILogger<DictionaryItemProvider> logger)
        {
            _connectionStringProvider = connectionStringProvider;
            _logger = logger;
        }

        public async Task<DictionaryItemDto> GetAsync(string id)
        {
            var connectionString = await _connectionStringProvider.GetConnectionString();
            await using var connection = new SqlConnection(connectionString);
            
            var parameters = new Dictionary<string, object>{{"@Id", id}};
            var item = await connection.QuerySingleOrDefaultAsync<DictionaryItemDto>("GetDictionaryItem",
                parameters, commandType: CommandType.StoredProcedure);
            if (item == null)
            {
                _logger.LogDebug("Could not find a dictionary item with id '{0}'", id);

                return null;
            }

            return item;
        }

        public async Task<IReadOnlyList<DictionaryListItemDto>> GetListAsync(CultureInfo culture)
        {
            var connectionString = await _connectionStringProvider.GetConnectionString();
            await using var connection = new SqlConnection(connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@CultureName", culture.Name);
            var items = await connection.QueryAsync<DictionaryListItemDto>("GetDictionaryItems",
                parameters, commandType: CommandType.StoredProcedure);

            return items.ToList();
        }
    }
}
