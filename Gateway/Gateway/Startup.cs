using Gateway.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Gateway
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var servicesConfig = new Services();
            _configuration.GetSection("Services").Bind(servicesConfig);
            services.AddSingleton(p => servicesConfig);

            services.AddHttpClient<HttpClient>(client => client.Timeout = TimeSpan.FromSeconds(5));
            services.AddScoped<IHttpClientFactory, ProxyClientFactory>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Services services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                var path = context.Request.Path.Value;
                string destination;
                if (path.StartsWith("/api/dictionary", StringComparison.InvariantCultureIgnoreCase))
                {
                    destination = services.DictionaryUrl + path.Replace("/dictionary", string.Empty);
                }
                else if (path.StartsWith("/api/auth", StringComparison.InvariantCultureIgnoreCase))
                {
                    destination = services.UsersUrl + path.Replace("/auth", string.Empty);
                }
                else if (path.StartsWith("/api/pages", StringComparison.InvariantCultureIgnoreCase))
                {
                    destination = services.PagesUrl + path;
                }
                else
                {
                    context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                    return;
                }

                var clientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
                var client = clientFactory.CreateClient();

                var request = context.Request;
                var requestMessage = new HttpRequestMessage(new HttpMethod(request.Method), destination);
                if (request.ContentLength.HasValue)
                {
                    requestMessage.Content = new StreamContent(request.Body, (int)request.ContentLength.Value);
                }

                foreach (var (key, values) in request.Headers)
                {
                    requestMessage.Headers.Add(key, values.ToArray());
                }

                var responseMessage = await client.SendAsync(requestMessage);
                context.Response.Body = await responseMessage.Content.ReadAsStreamAsync();
                context.Response.StatusCode = (int)responseMessage.StatusCode;

                foreach (var (key, values) in responseMessage.Headers)
                {
                    context.Response.Headers.Add(key, new StringValues(values.ToArray()));
                }
            });
        }
    }

    public class ProxyClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _client;

        public ProxyClientFactory(HttpClient client)
        {
            _client = client;
        }

        public HttpClient CreateClient(string name)
        {
            return _client;
        }
    }
}
