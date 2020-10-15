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
    internal class UserProvider : IUserProvider
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly ILogger<UserProvider> _logger;

        public UserProvider(
            IConnectionStringProvider connectionStringProvider,
            ILogger<UserProvider> logger)
        {
            _connectionStringProvider = connectionStringProvider;
            _logger = logger;
        }

        public async Task<UserDto> GetAsync(string id)
        {
            _logger.LogDebug("Getting user with id '{0}'.", id);

            var connectionString = await _connectionStringProvider.GetConnectionString();
            await using var connection = new SqlConnection(connectionString);

            var parameters = new Dictionary<string, object>
            {
                {"@UserId", id }
            };
            var user = await connection.QuerySingleOrDefaultAsync<UserDto>("GetUser", parameters, commandType: CommandType.StoredProcedure);
            if (user == null)
            {
                _logger.LogDebug("No user was found with id '{0}'", id);

                throw new NotFoundException(ErrorMessages.UserNotFound);
            }

            _logger.LogDebug("Successfully retrieved user by id '{0}'", id);

            return user;
        }

        public async Task<IReadOnlyList<UserDto>> GetListAsync(string searchTerm = null, string roleId = null)
        {
            _logger.LogDebug("Getting users with term '{0}' and role '{1}'.", searchTerm, roleId);

            var connectionString = await _connectionStringProvider.GetConnectionString();
            await using var connection = new SqlConnection(connectionString);

            var parameters = new Dictionary<string, object>
            {
                {"@SearchTerm", searchTerm == null ? null : $"%{searchTerm}%" },
                {"@RoleId", roleId }
            };

            var users = await connection.QueryAsync<UserDto>("GetUsers", parameters, commandType: CommandType.StoredProcedure);
            return users.ToList();
        }
    }
}
