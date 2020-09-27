using Microsoft.Extensions.DependencyInjection;
using Shared.Localization.Providers;

namespace Shared.Localization.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalizer(this IServiceCollection services)
        {
            return services
                .AddTransient<ICultureProvider, CultureProvider>()
                .AddTransient<ILocalizer, Localizer>();
        }
    }
}
