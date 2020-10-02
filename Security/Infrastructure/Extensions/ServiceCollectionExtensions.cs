using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<AuthService>(client => client.Timeout = TimeSpan.FromSeconds(5));
            services.AddTransient<IAuthService, AuthService>();
            
            services.Configure<TokenOptions>(tokenOptions => configuration.GetSection("Token").Bind(tokenOptions));
            services.AddTransient<ITokenService, TokenService>();
        }
    }
}
