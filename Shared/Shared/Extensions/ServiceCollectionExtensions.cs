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
            var passwordOptions = new PasswordOptions();
            options.Invoke(passwordOptions);

            services.Configure<PasswordValidationOptions>(o => o = passwordOptions.Validation);
            services.Configure<PasswordHasherOptions>(o => o = passwordOptions.Hasher);

            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IPasswordValidator, PasswordValidator>();
        }
    }
}
