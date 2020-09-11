using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pages.Domain.Models;
using Pages.Infrastructure;
using Pages.Infrastructure.Providers;
using Pages.Infrastructure.Services;
using Shared;
using Shared.Entity;

namespace Pages.Infrastructure
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
