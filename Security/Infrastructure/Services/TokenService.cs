using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using Shared.Exceptions;
using Shared.Extensions;
using Shared.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Shared.OAuth;

namespace Infrastructure.Services
{
    internal class TokenService : ITokenService
    {
        private readonly TokenOptions _tokenOptions;
        private readonly SecurityClients _securityClients;
        private readonly ISystemClock _systemClock;

        public TokenService(
            IOptions<TokenOptions> tokenOptions, 
            SecurityClients securityClients,
            ISystemClock systemClock)
        {
            _tokenOptions = tokenOptions?.Value ??
                            throw new InvalidOperationException("TokenOptions must be configured.");
            _securityClients = securityClients;
            _systemClock = systemClock;
        }

        public async Task<Token> GenerateAsync(IReadOnlyList<ClaimDto> claims)
        {
            var header = Serialize(new Dictionary<string, string>
            {
                { "alg", "RSA256" },
                { "typ", "JWT" },
                { "kid", Guid.NewGuid().ToString() }
            });
            var (tokenClaims, expiryTimestamp) = BuildClaims(claims);
            var payload = Serialize(tokenClaims);
            var data = new byte[header.Length + 1 + payload.Length];

            Buffer.BlockCopy(header, 0, data, 0, header.Length);
            data[header.Length] = (byte)'.';
            Buffer.BlockCopy(payload, 0, data, header.Length + 1, payload.Length);

            return new Token
            {
                AccessToken = await SignAndReturnTokenAsync(data),
                Expires = expiryTimestamp
            };

            static byte[] Serialize(object value)
            {
                var json = JsonSerializer.SerializeToUtf8Bytes(value);
                var base64 = WebEncoders.Base64UrlEncode(json);
                return Encoding.UTF8.GetBytes(base64);
            }
        }

        public async Task<string> VerifyAsync(string accessToken, string clientId)
        {
            var lastDot = accessToken.LastIndexOf('.');

            var tokenData = Encoding.UTF8.GetBytes(accessToken.Substring(0, lastDot));
            var signature = WebEncoders.Base64UrlDecode(accessToken.Substring(lastDot + 1));

            using var alg = SHA256.Create();
            var digest = alg.ComputeHash(tokenData);
            using var rsa = await LoadPrivateKeyAsync();

            if (!rsa.VerifyHash(digest, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1))
            {
                throw new AuthenticationFailedException("Token is invalid.");
            }

            var payloadBase64 = accessToken.Split('.')[1];
            var payload = JwtPayload.Base64UrlDeserialize(payloadBase64);
            
            // Validate 'Not before'
            if (Convert.ToDouble(payload["nbf"]) > _systemClock.UtcNow.Unix())
            {
                throw new AuthenticationFailedException("Token is not yet valid.");
            }

            // Validate 'Expiry'
            if (Convert.ToDouble(payload["exp"]) < _systemClock.UtcNow.Unix())
            {
                throw new AuthenticationFailedException("Token has expired.");
            }

            var clientSecret = _securityClients[clientId];
            using var checkAlg = SHA1.Create();
            var hash = checkAlg.ComputeHash(Encoding.UTF8.GetBytes(accessToken + clientSecret));

            return Convert.ToBase64String(hash);
        }

        private (IDictionary<string, object> Claims, double ExpiryTimestamp) BuildClaims(IReadOnlyList<ClaimDto> userClaims)
        {
            var timeNow = _systemClock.UtcNow;
            var expiryDate = timeNow.AddSeconds(_tokenOptions.ExpirySeconds);

            var claims = new Dictionary<string, object>
            {
                { "exp", expiryDate.Unix() },
                { "iat", timeNow.Unix() },
                { "nbf", timeNow.Unix() },
                { "iss", _tokenOptions.Issuer }
            };

            foreach (var commonClaims in userClaims.GroupBy(x => x.Key))
            {
                if (commonClaims.Count() > 1)
                {
                    claims[commonClaims.Key] = commonClaims.Select(x => x.Value);
                }
                else
                {
                    claims[commonClaims.Key] = commonClaims.First().Value;
                }
            }

            return (claims, expiryDate.Unix());
        }

        /// <summary>
        /// Signs a hash of <paramref name="data"/> and returns a new array in the format of {data}.{signature}
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<string> SignAndReturnTokenAsync(byte[] data)
        {
            using var alg = SHA256.Create();
            var digest = alg.ComputeHash(data);

            using var rsa = await LoadPrivateKeyAsync();
            var signature = rsa.SignHash(digest, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var base64 = Encoding.UTF8.GetBytes(WebEncoders.Base64UrlEncode(signature));

            var signedData = new byte[data.Length + 1 + base64.Length];
            Buffer.BlockCopy(data, 0, signedData, 0, data.Length);
            signedData[data.Length] = (byte) '.';
            Buffer.BlockCopy(base64, 0, signedData, data.Length + 1, base64.Length);

            return Encoding.UTF8.GetString(signedData);
        }

        private async Task<RSA> LoadPrivateKeyAsync()
        {
            var filepath = Environment.GetEnvironmentVariable(Constants.RsaKeyFileVariable);
            if (string.IsNullOrEmpty(filepath))
            {
                throw new InvalidOperationException($"The environment variable '{Constants.RsaKeyFileVariable}' must be set.");
            }

            using var reader = new StreamReader(filepath, Encoding.UTF8);
            var data = await reader.ReadToEndAsync();

            var fi = data.IndexOf('\n');
            var li = data[..data.LastIndexOf('\n')].LastIndexOf('\n');
            var pemData = data.Substring(fi + 1, li - fi - 1);
            
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(pemData), out _);

            return rsa;
        }
    }
}
