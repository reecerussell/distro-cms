using Dictionary.Infrastructure.Providers;
using Dictionary.Infrastructure.Repositories;
using Dictionary.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Dictionary.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var connectionStringProvider =
                services.BuildServiceProvider().GetRequiredService<IConnectionStringProvider>();
            var connectionString = connectionStringProvider.GetConnectionString().Result;

            return services
                .AddDbContext<DictionaryContext>(options =>
                    options.UseSqlServer(connectionString))
                .AddScoped<DbContext>(p => p.GetRequiredService<DictionaryContext>())
                .AddTransient<ISupportedCultureRepository, SupportedCultureRepository>()
                .AddTransient<ISupportedCultureService, SupportedCultureService>()
                .AddTransient<IDictionaryItemRepository, DictionaryItemRepository>()
                .AddTransient<IDictionaryService, DictionaryService>()
                .AddTransient<IDictionaryItemProvider, DictionaryItemProvider>();
        }
    }
}
