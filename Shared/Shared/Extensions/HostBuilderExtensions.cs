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
                var devFilename = filename + ".development.json";
                if (File.Exists(devFilename))
                {
                    builder.AddJsonFile(devFilename);
                    return;
                }
            }

            var defaultFilename = filename + ".json";
            if (!File.Exists(defaultFilename))
            {
                return;
            }

            builder.AddJsonFile(defaultFilename);
        }
    }
}
