using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddShared(this IServiceCollection services)
        {
            services.AddTransient<IConnectionStringProvider, ConnectionStringProvider>();
        }
    }
}
