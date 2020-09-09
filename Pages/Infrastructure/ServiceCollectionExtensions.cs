using Domain.Models;
using Infrastructure.Providers;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using Shared.Entity;

namespace Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var connectionStringProvider = services.BuildServiceProvider().GetRequiredService<IConnectionStringProvider>();
            var connectionString = connectionStringProvider.GetConnectionString().Result;

            return services
                .AddDbContext<PagesContext>(options =>
                    {
                        options.UseSqlServer(connectionString, 
                            x => x.MigrationsAssembly("Migrations"));
                    })
                .AddScoped<DbContext>(provider => provider.GetRequiredService<PagesContext>())
                .AddScoped<IRepository<Page>, Repository<Page>>()
                .AddTransient<IPageService, PageService>()
                .AddTransient<IPageProvider, PageProvider>();
        }
    }
}
