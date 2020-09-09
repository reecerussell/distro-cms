using Dapper;
using Domain.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Exceptions;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;

namespace Infrastructure.Providers
{
    internal class PageProvider : IPageProvider
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly ILogger<PageProvider> _logger;

        public PageProvider(
            IConnectionStringProvider connectionStringProvider,
            ILogger<PageProvider> logger)
        {
            _connectionStringProvider = connectionStringProvider;
            _logger = logger;
        }

        public async Task<PageDto> GetPageAsync(string id, CultureInfo culture)
        {
            _logger.LogDebug("Attempting to retrieve page '{0}' from the database for the culture: {1}", id, culture.Name);

            const string query = "GetPage";
            var parameters = new Dictionary<string, object>
            {
                {"@PageId", id},
                {"@CultureName", culture.Name}
            };

            await using var connection = new SqlConnection(await _connectionStringProvider.GetConnectionString());
            var page = await connection.QuerySingleOrDefaultAsync<PageDto>(query, parameters, commandType: CommandType.StoredProcedure);
            if (page == null)
            {
                throw new NotFoundException(ErrorMessages.PageNotFound);
            }

            return page;
        }
    }
}
