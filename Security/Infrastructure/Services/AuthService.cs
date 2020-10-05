using Microsoft.Extensions.Logging;
using Shared;
using Shared.Controllers;
using Shared.Exceptions;
using Shared.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    internal class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IHttpClientFactory clientFactory,
            ILogger<AuthService> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public Task<IReadOnlyList<ClaimDto>> AuthenticateAsync(SecurityCredential credential)
        {
            return credential.GrantType.ToLower() switch
            {
                "password" => AuthenticatePasswordAsync(credential.Email, credential.Password, credential.Audience),
                _ => throw new InvalidOperationException($"The grant type '{credential.GrantType}' is not handled.")
            };
        }

        private async Task<IReadOnlyList<ClaimDto>> AuthenticatePasswordAsync(string email, string password, string audience)
        {
            var usersBaseUrl = Environment.GetEnvironmentVariable(Constants.UsersUrlVariable);
            if (string.IsNullOrEmpty(usersBaseUrl))
            {
                throw new InvalidOperationException($"The '{Constants.UsersUrlVariable}' environment variable is not set.");
            }

            await using var dataStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(dataStream, new PasswordGrantData(email, password, audience));
            var utf8Json = Encoding.UTF8.GetString(dataStream.ToArray());
            var content = new StringContent(utf8Json, Encoding.UTF8, "application/json");

            var targetUrl = usersBaseUrl + "/api/auth/password";
            var client = _clientFactory.CreateClient("default");
            var response = await client.PostAsync(targetUrl, content);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogWarning("Received a non-ok status code ({0}) from the password validation API.", (int)response.StatusCode);
                
                throw new AuthenticationFailedException(ErrorMessages.AuthFailedToVerify);
            }

            await using var contentStream = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonSerializer.DeserializeAsync<ResponseData<IReadOnlyList<ClaimDto>>>(contentStream, 
                new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

            _logger.LogDebug("Auth response data: {0}", JsonSerializer.Serialize(responseData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));

            switch (responseData.StatusCode)
            {
                case (int)HttpStatusCode.BadRequest:
                    throw new AuthenticationFailedException(ErrorMessages.AuthInvalidCredentials);
                case (int)HttpStatusCode.OK:
                    return responseData.Data;
                default:
                    _logger.LogWarning("Received a non-ok status code ({0}) from the password validation API, with error: {1}", responseData.StatusCode, responseData.Error);
                    throw new AuthenticationFailedException(ErrorMessages.AuthFailedToVerify);
            }
        }
    }
}
