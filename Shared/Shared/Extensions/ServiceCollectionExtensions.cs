using Microsoft.Extensions.DependencyInjection;
using Shared.Passwords;
using System;

namespace Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddShared(this IServiceCollection services)
        {
            services.AddTransient<IConnectionStringProvider, ConnectionStringProvider>();
        }

        public static void AddPasswords(this IServiceCollection services, Action<PasswordOptions> options)
        {
            services.Configure(options);
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IPasswordValidator, PasswordValidator>();
        }
    }
}
