using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Shared
{
    internal class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _configuration;

        public ConnectionStringProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<string> GetConnectionString()
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            if (!string.IsNullOrEmpty(connectionString))
            {
                return Task.FromResult(connectionString);
            }

            return GetConnectionString("DefaultConnection");
        }

        public Task<string> GetConnectionString(string name)
        {
            return Task.FromResult(_configuration.GetConnectionString(name));
        }
    }
}
