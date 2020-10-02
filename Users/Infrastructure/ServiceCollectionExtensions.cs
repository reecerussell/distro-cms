using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using Users.Infrastructure.Providers;
using Users.Infrastructure.Repositories;
using Users.Infrastructure.Services;

namespace Users.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var connectionStringProvider = services.BuildServiceProvider().GetRequiredService<IConnectionStringProvider>();
            var connectionString = connectionStringProvider.GetConnectionString().Result;

            return services
                .AddDbContext<UserContext>(options => { options.UseSqlServer(connectionString); })
                .AddScoped<DbContext>(s => s.GetRequiredService<UserContext>())
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IRoleRepository, RoleRepository>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IUserProvider, UserProvider>()
                .AddTransient<IRoleService, RoleService>()
                .AddTransient<IRoleProvider, RoleProvider>()
                .AddTransient<IAuthService, AuthService>();
        }
    }
}
