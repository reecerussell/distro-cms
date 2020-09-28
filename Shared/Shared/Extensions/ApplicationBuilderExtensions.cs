using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Shared.Middleware;
using System.Globalization;

namespace Shared.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomRequestLocalization(this IApplicationBuilder app)
        {
            return app.UseRequestLocalization(async options =>
            {
                options.DefaultRequestCulture = new RequestCulture(CultureInfo.CurrentCulture);
                options.SupportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
                options.RequestCultureProviders.Insert(0, new HeaderRequestCultureProvider());
            });
        }
    }
}
