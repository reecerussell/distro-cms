using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Extensions;
using Shared.Middleware;
using System.Globalization;
using System.Text.Json;

namespace API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Custom services
            services.AddShared();
            services.AddInfrastructure();

            // Extensions
            services.AddLocalization();
            services.AddLogging(options => options.AddConsole());

            // MVC/API
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.AllowTrailingCommas = false;
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Ensure the database is up to date.
            context.Database.EnsureCreated();

            app.UseRouting();

            app.UseRequestLocalization(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-GB");
                options.SupportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
                options.RequestCultureProviders.Insert(0, new HeaderRequestCultureProvider());
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("defaultApi", "api/{controller}/{id?}");
            });
        }
    }
}