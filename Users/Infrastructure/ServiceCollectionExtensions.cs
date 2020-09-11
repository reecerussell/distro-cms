using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var connectionStringProvider = services.BuildServiceProvider().GetRequiredService<IConnectionStringProvider>();
            var connectionString = connectionStringProvider.GetConnectionString().Result;

            return services
                .AddDbContext<UserContext>(options =>
                {
                    options.UseSqlServer(connectionString, x => x.MigrationsAssembly("Migrations"));
                })
                .AddScoped<DbContext>(s => s.GetRequiredService<UserContext>())
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IUserService, UserService>();
        }
    }
}
