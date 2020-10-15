using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Exceptions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Users.Domain.Dtos;

namespace Users.Infrastructure.Providers
{
    internal class RoleProvider : IRoleProvider
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly ILogger<RoleProvider> _logger;

        public RoleProvider(
            IConnectionStringProvider connectionStringProvider,
            ILogger<RoleProvider> logger)
        {
            _connectionStringProvider = connectionStringProvider;
            _logger = logger;
        }

        public async Task<IReadOnlyList<RoleListItemDto>> GetListAsync(string term)
        {
            var connectionString = await _connectionStringProvider.GetConnectionString();
            await using var connection = new SqlConnection(connectionString);

            var parameters = new Dictionary<string, object>{{"@SearchTerm", $"%{term}%"}};
            var roles = (await connection.QueryAsync<RoleListItemDto>("GetRoles", parameters, 
                commandType: CommandType.StoredProcedure)).ToList();

            _logger.LogDebug("Found {0} roles with search term '{1}'", roles.Count, term);

            return roles;
        }

        public async Task<RoleDto> GetAsync(string id)
        {
            var connectionString = await _connectionStringProvider.GetConnectionString();
            await using var connection = new SqlConnection(connectionString);

            var parameters = new Dictionary<string, object>{{"@Id", id}};
            var role = await connection.QuerySingleOrDefaultAsync<RoleDto>("GetRole",
                parameters, commandType: CommandType.StoredProcedure);
            if (role == null)
            {
                _logger.LogDebug("Could not find role with id '{0}'", id);

                throw new NotFoundException(ErrorMessages.RoleNotFound);
            }

            return role;
        }

        public async Task<IReadOnlyList<RoleDropdownDto>> GetDropdownItemsAsync()
        {
            var connectionString = await _connectionStringProvider.GetConnectionString();
            await using var connection = new SqlConnection(connectionString);

            var roles = await connection.QueryAsync<RoleDropdownDto>("GetRolesForDropdown",
                commandType: CommandType.StoredProcedure);

            return roles.ToList();
        }
    }
}
