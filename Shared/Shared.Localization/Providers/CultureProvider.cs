using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.Localization.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Localization.Providers
{
    internal class CultureProvider : ICultureProvider
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CultureProvider> _logger;

        public CultureProvider(
            IConnectionStringProvider connectionStringProvider,
            IMemoryCache cache,
            ILogger<CultureProvider> logger)
        {
            _connectionStringProvider = connectionStringProvider;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IReadOnlyList<CultureDto>> GetCultures()
        {
            _logger.LogDebug("Getting cultures...");

            const string cacheKey = "Localizer:SupportedCultures";
            if (_cache.TryGetValue(cacheKey, out IReadOnlyList<CultureDto> cachedCultures))
            {
                _logger.LogDebug("Using {0} cached cultures.", cachedCultures.Count);

                return cachedCultures;
            }

            var connectionString = await _connectionStringProvider.GetConnectionString();
            await using var connection = new SqlConnection(connectionString);
            var cultures =
                (await connection.QueryAsync<CultureDto>("GetCulturesForSetup",
                    commandType: CommandType.StoredProcedure)).ToList();

            _logger.LogDebug("Using {0} cultures from the database.", cultures.Count);

            _cache.Set(cacheKey, cultures, TimeSpan.FromMinutes(1));

            return cultures;
        }
    }
}
