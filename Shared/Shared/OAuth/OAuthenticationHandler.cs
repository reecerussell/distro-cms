using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Controllers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.OAuth
{
    internal class OAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<OAuthenticationHandler> _logger;
        private readonly SecurityClientInfo _securityClient;

        public OAuthenticationHandler(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache,
            ILogger<OAuthenticationHandler> logger,
            SecurityClientInfo securityClientInfo,
            IOptionsMonitor<AuthenticationSchemeOptions> optionsMonitor,
            ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder,
            ISystemClock systemClock)
            : base(optionsMonitor, loggerFactory, urlEncoder, systemClock)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
            _logger = logger;
            _securityClient = securityClientInfo;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            _logger.LogDebug("Handling authentication for OAUTH...");

            var accessToken = GetAccessToken(_httpContextAccessor.HttpContext);
            if (accessToken == null)
            {
                _logger.LogDebug("No bearer token was present on request.");
                return AuthenticateResult.Fail("You're missing the secret password!");
            }

            var error = await InternalAuthOperationAsync(accessToken);
            if (error != null)
            {
                return AuthenticateResult.Fail(error);
            }

            return BuildTicket(accessToken);

            static AuthenticateResult BuildTicket(string token)
            {
                var identity = new ClaimsIdentity(GetTokenClaims(token), OAuthConstants.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                return AuthenticateResult.Success(new AuthenticationTicket(principal, OAuthConstants.AuthenticationScheme));
            }
        }

        private async Task<string> InternalAuthOperationAsync(string accessToken)
        {
            _logger.LogDebug("Handling authentication for OAUTH...");
            
            var cacheKey = "OAUTH:TOKEN:" + accessToken;
            if (_memoryCache.TryGetValue(cacheKey, out bool cachedResult))
            {
                _logger.LogDebug("Using cached token result: {0}", cachedResult);

                if (!cachedResult)
                {
                    return "Your access token is invalid and has expired!";
                }

                // Success
                return null;
            }

            var result = await ValidateTokenAsync(accessToken);
            if (!result)
            {
                _logger.LogDebug("Access token is invalid, caching failed result.");

                _memoryCache.Set(cacheKey, false, TimeSpan.FromHours(1));

                return "You're missing the secret password!";
            }

            _logger.LogDebug("Access token is valid, caching successful result.");

            _memoryCache.Set(cacheKey, true, TimeSpan.FromMinutes(5));

            // Success;
            return null;
        }

        private async Task<bool> ValidateTokenAsync(string accessToken)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "oauth/verify");
            var payload = new VerifyTokenRequest
            {
                AccessToken = accessToken,
                ClientId = _securityClient.Id
            };
            await using var jsonStream = new MemoryStream();
            var jsonOptions = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            await JsonSerializer.SerializeAsync(jsonStream, payload, jsonOptions);
            var utf8Json = Encoding.UTF8.GetString(jsonStream.ToArray());
            requestMessage.Content = new StringContent(utf8Json, Encoding.UTF8, "application/json");
            
            try
            {
                var client = _httpClientFactory.CreateClient(OAuthConstants.HttpClientName);
                var response = await client.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();

                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var responseData = await JsonSerializer.DeserializeAsync<ResponseData<VerifyTokenResponse>>(responseStream, jsonOptions);
                _logger.LogDebug("Response JSON: {0}\n", JsonSerializer.Serialize(responseData));

                if (responseData.StatusCode != (int) HttpStatusCode.OK)
                {
                    _logger.LogWarning("Verification response had status code {0}, with error '{1}'.", responseData.StatusCode, responseData.Error);

                    return false;
                }

                return IsResponseValid(accessToken, _securityClient.Secret, responseData.Data.Checksum);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Authentication failed.");

                return false;
            }
        }

        private static bool IsResponseValid(string accessToken, string clientSecret, string base64Hash)
        {
            using var alg = SHA1.Create();
            var localHash = alg.ComputeHash(Encoding.UTF8.GetBytes(accessToken + clientSecret));
            var hash = Convert.FromBase64String(base64Hash);

            return CryptographicOperations.FixedTimeEquals(localHash, hash);
        }

        private static string GetAccessToken(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var headerValues))
            {
                return null;
            }

            var value = headerValues.FirstOrDefault();
            if (value == null)
            {
                return null;
            }

            if (!value.StartsWith("Bearer"))
            {
                return null;
            }

            return value[7..];
        }

        private static IReadOnlyList<Claim> GetTokenClaims(string accessToken)
        {
            var tokenParts = accessToken.Split('.');

            return JwtPayload.Base64UrlDeserialize(tokenParts[1]).Claims.ToList();
        }
    }
}
