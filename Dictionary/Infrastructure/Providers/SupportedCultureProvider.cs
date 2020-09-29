using Dapper;
using Dictionary.Domain.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Exceptions;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary.Infrastructure.Providers
{
    internal class SupportedCultureProvider : ISupportedCultureProvider
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly ILogger<SupportedCultureProvider> _logger;

        public SupportedCultureProvider(
            IConnectionStringProvider connectionStringProvider,
            ILogger<SupportedCultureProvider> logger)
        {
            _connectionStringProvider = connectionStringProvider;
            _logger = logger;
        }

        public async Task<SupportedCultureDto> GetAsync(string id)
        {
            var connectionString = await _connectionStringProvider.GetConnectionString();
            await using var connection = new SqlConnection(connectionString);

            var parameters = new Dictionary<string, object> {{"@Id", id}};
            var culture = await connection.QuerySingleOrDefaultAsync<SupportedCultureDto>(
                "GetSupportedCulture", parameters, commandType: CommandType.StoredProcedure);
            if (culture == null)
            {
                _logger.LogDebug("Failed to get a supported culture as one could not be found with id '{0}'", id);

                throw new NotFoundException(ErrorMessages.SupportedCultureNotFound);
            }

            return culture;
        }

        public async Task<IReadOnlyList<SupportedCultureDropdownItemDto>> GetDropdownItemsAsync()
        {
            var connectionString = await _connectionStringProvider.GetConnectionString();
            await using var connection = new SqlConnection(connectionString);

            return (await connection.QueryAsync<SupportedCultureDropdownItemDto>(
                "GetCulturesForDropdown", commandType: CommandType.StoredProcedure)).ToList();
        }

        public async Task<IReadOnlyList<SupportedCultureDropdownItemDto>> GetAvailableDropdownItemsAsync()
        {
            var supportedCultures = (await GetDropdownItemsAsync()).Select(x => x.Name).ToList();

            return CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(x => !string.IsNullOrEmpty(x.Name) && !supportedCultures.Contains(x.Name))
                .Select(x => new SupportedCultureDropdownItemDto{Name = x.Name, DisplayName = x.DisplayName})
                .ToList();
        }
    }
}
