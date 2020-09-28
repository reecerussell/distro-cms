using Microsoft.Extensions.DependencyInjection;

namespace Shared.Localization.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalizer(this IServiceCollection services)
        {
            return services
                .AddTransient<ILocalizer, Localizer>();
        }
    }
}
