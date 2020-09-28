using Dapper;
using Dictionary.Domain.Dtos;
using Microsoft.Data.SqlClient;
using Shared;
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

        public SupportedCultureProvider(
            IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IReadOnlyList<SupportedCultureDropdownItemDto>> GetDropdownItemsAsync(CultureInfo culture)
        {
            var connectionString = await _connectionStringProvider.GetConnectionString();
            await using var connection = new SqlConnection(connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@CultureName", culture.Name);

            return (await connection.QueryAsync<SupportedCultureDropdownItemDto>("GetCulturesForDropdown", parameters,
                commandType: CommandType.StoredProcedure)).ToList();
        }
    }
}
