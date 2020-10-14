using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shared.Localization
{
    internal class Localizer : ILocalizer
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IMemoryCache _cache;
        private readonly ILogger<Localizer> _logger;

        public Localizer(
            IConnectionStringProvider connectionStringProvider,
            IMemoryCache cache,
            ILogger<Localizer> logger)
        {
            _connectionStringProvider = connectionStringProvider;
            _cache = cache;
            _logger = logger;
        }

        public async Task<string> GetStringAsync(string key)
        {
            _logger.LogDebug("Localizing string for key '{0}' and culture '{1}'", key, CultureInfo.CurrentCulture.Name);

            var cacheKey = $"Localizer:{key}";
            if (_cache.TryGetValue(cacheKey, out string cachedValue))
            {
                _logger.LogDebug("Using cached value '{0}' for string: '{1}'", cachedValue, key);

                return cachedValue;
            }

            var connectionString = await _connectionStringProvider.GetConnectionString();
            await using var connection = new SqlConnection(connectionString);
            var parameters = new Dictionary<string, object>
            {
                { "@Key", key },
                { "@CultureName", CultureInfo.CurrentCulture.Name }
            };

            var value = await connection.QuerySingleOrDefaultAsync<string>("GetDictionaryString", parameters,
                commandType: CommandType.StoredProcedure);

            _logger.LogDebug("Using database value '{0}' for string '{1}'", value, key);
            _logger.LogDebug("Caching value for 5 minutes.");

            _cache.Set(cacheKey, value, TimeSpan.FromMinutes(5));

            return value;
        }

        public async Task<string> GetErrorAsync(string key)
        {
            var errorMessages = typeof(ErrorMessages).GetFields()
                .Where(x => x.IsPublic && x.FieldType == typeof(string))
                .Select(x => (string)x.GetRawConstantValue())
                .ToList();

            var localizedString = await GetStringAsync(key);
            if (localizedString != null)
            {
                return localizedString;
            }

            foreach (var message in errorMessages)
            {
                var pattern = Regex.Replace(message, "({[0-9]+?})", "(.*[^:])");
                var match = Regex.Match(key, pattern);
                if (!match.Success)
                {
                    continue;
                }

                localizedString = await GetStringAsync(message);
                var captureArgs = match.Groups.Values
                    .Skip(1)
                    .SelectMany(x => x.Captures.Select(y => y.Value))
                    .ToList();
                if (captureArgs.Count < 1)
                {
                    return localizedString ?? key;
                }

                return string.Format(localizedString, captureArgs);
            }

            return key;
        }
    }
}
