using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Shared.Middleware;
using System.Globalization;
using System.Linq;

namespace Shared.Localization.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomRequestLocalization(this IApplicationBuilder app)
        {
            var cultureProvider = app.ApplicationServices.GetRequiredService<ICultureProvider>();

            return app.UseRequestLocalization(async options =>
            {
                var cultures = await cultureProvider.GetCultures();

                try
                {
                    var defaultCulture = cultures.FirstOrDefault(x => x.IsDefault) ??
                                         throw new CultureNotFoundException();
                    options.DefaultRequestCulture = new RequestCulture(defaultCulture.Name);
                }
                catch (CultureNotFoundException)
                {
                    options.DefaultRequestCulture = new RequestCulture(CultureInfo.CurrentCulture);
                }

                options.SupportedCultures = cultures.Select(x => new CultureInfo(x.Name)).ToList();
                options.RequestCultureProviders.Insert(0, new HeaderRequestCultureProvider());
            });
        }
    }
}
