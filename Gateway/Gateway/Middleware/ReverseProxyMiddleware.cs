﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gateway.Middleware
{
    // https://www.iaspnetcore.com/blog/blogpost/5eabff60a9bd6d01e67436f4
    public class ReverseProxyMiddleware : IMiddleware
    {
        private readonly ILogger<ReverseProxyMiddleware> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public ReverseProxyMiddleware(
            ILogger<ReverseProxyMiddleware> logger, 
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var targetUri = GetTargetUri(context);
            if (targetUri == null)
            {
                _logger.LogInformation("Missed reverse proxy.");

                await next(context);
                return;
            }

            _logger.LogInformation("Using reverse proxy to send request to {0}", targetUri.ToString());

            var targetRequestMessage = CreateTargetMessage(context, targetUri);
            var httpClient = _httpClientFactory.CreateClient();
            using var responseMessage = await httpClient.SendAsync(targetRequestMessage,
                HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);

            context.Response.StatusCode = (int)responseMessage.StatusCode;

            // Headers
            foreach (var header in responseMessage.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
            //set each one of these headers
            foreach (var header in responseMessage.Content.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
            context.Response.Headers.Remove("transfer-encoding");

            // Body
            var content = await responseMessage.Content.ReadAsByteArrayAsync();
            await context.Response.Body.WriteAsync(content);
        }

        private HttpRequestMessage CreateTargetMessage(HttpContext context, Uri targetUri)
        {
            _logger.LogDebug("Creating proxy request message, to: {0}", targetUri);

            var requestMessage = new HttpRequestMessage();
            var streamContent = new StreamContent(context.Request.Body);
            requestMessage.Content = streamContent;
            requestMessage.RequestUri = targetUri;

            _logger.LogDebug("Headers:");
            foreach (var header in context.Request.Headers)
            {
                _logger.LogDebug("\t{0}: {1}", header.Key, string.Join(",", header.Value.ToArray()));
                requestMessage.Content.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }

            requestMessage.Headers.Host = targetUri.Host;

            _logger.LogDebug("Method: {0}", context.Request.Method);
            requestMessage.Method = new HttpMethod(context.Request.Method);

            return requestMessage;
        }

        private Uri GetTargetUri(HttpContext context)
        {
            Uri targetUri = null;

            var path = context.Request.Path.Value;
            if (path.StartsWith("/api/dictionary", StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogInformation("Path {0} matches /api/dictionary", path);

                var baseUrl = Environment.GetEnvironmentVariable(Constants.DictionaryUrlVariable);
                if (!string.IsNullOrEmpty(baseUrl))
                {
                    targetUri = new Uri(baseUrl + path.Replace("/dictionary", string.Empty) + context.Request.QueryString.Value);
                }
            }
            else if (path.StartsWith("/api/auth", StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogInformation("Path {0} matches /api/auth", path);

                var baseUrl = Environment.GetEnvironmentVariable(Constants.UsersUrlVariable);
                if (!string.IsNullOrEmpty(baseUrl))
                {
                    targetUri = new Uri(baseUrl + path.Replace("/auth", string.Empty) + context.Request.QueryString.Value);
                }
            }
            else if (path.StartsWith("/api/pages", StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogInformation("Path {0} matches /api/pages", path);

                var baseUrl = Environment.GetEnvironmentVariable(Constants.PagesUrlVariable);
                if (!string.IsNullOrEmpty(baseUrl))
                {
                    targetUri = new Uri(baseUrl + path + context.Request.QueryString.Value);
                }
            }
            else if (path.StartsWith("/oauth", StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogInformation("Path {0} matches /oauth", path);

                var baseUrl = Environment.GetEnvironmentVariable(Constants.SecurityUrlVariable);
                if (!string.IsNullOrEmpty(baseUrl))
                {
                    targetUri = new Uri(baseUrl + path + context.Request.QueryString.Value);
                }
            }

            _logger.LogInformation("Destination: {0}", targetUri);

            return targetUri;
        }
    }
}
