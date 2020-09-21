using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Shared.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureSharedConfiguration(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration((builder, config) =>
            {
                config.AddSettings(builder.HostingEnvironment, "appsettings");
                config.AddSettings(builder.HostingEnvironment, "settings");
            });
        }

        private static void AddSettings(this IConfigurationBuilder builder, IHostEnvironment environment, string filename)
        {
            if (environment.IsDevelopment())
            {
                filename += ".development.json";
            }
            else
            {
                filename += ".json";
            }

            if (!File.Exists(filename))
            {
                return;
            }

            builder.AddJsonFile(filename);
        }
    }
}
