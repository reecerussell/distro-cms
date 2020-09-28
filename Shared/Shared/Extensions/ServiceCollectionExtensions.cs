using Microsoft.Extensions.DependencyInjection;
using Shared.Localization;
using Shared.Passwords;
using System;

namespace Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddShared(this IServiceCollection services)
        {
            services.AddTransient<IConnectionStringProvider, ConnectionStringProvider>();
            services.AddTransient<ILocalizer, Localizer>();
        }

        public static void AddPasswords(this IServiceCollection services, Action<PasswordOptions> options)
        {
            services.Configure(options);
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IPasswordValidator, PasswordValidator>();
        }
    }
}
