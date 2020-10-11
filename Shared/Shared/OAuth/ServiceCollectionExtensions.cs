using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Shared.OAuth
{
    public static class ServiceCollectionExtensions
    {
        public static void AddOAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient(OAuthConstants.HttpClientName, client =>
            {
                client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("SECURITY_URL"));
                client.Timeout = TimeSpan.FromSeconds(5);
            });
            services.AddHttpContextAccessor();

            // Security clients
            var clients = new List<SecurityClientInfo>();
            configuration.GetSection("Clients").Bind(clients);
            services.AddSingleton(new SecurityClients(clients));

            // Security client
            var client = new SecurityClientInfo();
            configuration.GetSection("Security:Client").Bind(client);
            services.AddSingleton(client);

            //services.AddAuthorization();
            services.AddAuthentication(options =>
            {
                options.AddScheme<OAuthenticationHandler>(OAuthConstants.AuthenticationScheme, "OAuth");

                options.DefaultAuthenticateScheme = OAuthConstants.AuthenticationScheme;
                options.DefaultChallengeScheme = OAuthConstants.AuthenticationScheme;
            });
        }
    }
}
