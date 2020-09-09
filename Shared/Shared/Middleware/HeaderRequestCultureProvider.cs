using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Middleware
{
    public class HeaderRequestCultureProvider : IRequestCultureProvider
    {
        public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var culture = httpContext.Request.Headers["API-Culture"].FirstOrDefault();
            if (string.IsNullOrEmpty(culture))
            {
                return Task.FromResult(default(ProviderCultureResult));
            }

            return Task.FromResult(new ProviderCultureResult(culture, culture));
        }
    }
}
