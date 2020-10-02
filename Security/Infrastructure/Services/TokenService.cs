using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using Shared.Extensions;
using Shared.Security;

namespace Infrastructure.Services
{
    internal class TokenService : ITokenService
    {
        private readonly TokenOptions _tokenOptions;
        private readonly ISystemClock _systemClock;

        public TokenService(IOptions<TokenOptions> tokenOptions, ISystemClock systemClock)
        {
            _tokenOptions = tokenOptions?.Value ??
                            throw new InvalidOperationException("TokenOptions must be configured.");
            _systemClock = systemClock;
        }

        public async Task<Token> GenerateAsync(IReadOnlyList<Claim> claims)
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
                var base64 = new byte[Base64.GetMaxEncodedToUtf8Length(json.Length)];
                Base64.EncodeToUtf8(json, base64, out _, out _);
                return base64;
            }
        }

        private (IReadOnlyList<Claim> Claims, double ExpiryTimestamp) BuildClaims(IReadOnlyList<Claim> userClaims)
        {
            var claims = new List<Claim>();

            var timeNow = _systemClock.UtcNow;
            var expiryDate = timeNow.AddSeconds(_tokenOptions.ExpirySeconds);

            claims.Add(new Claim("exp", expiryDate.Unix().ToString(CultureInfo.InvariantCulture)));
            claims.Add(new Claim("iat", timeNow.Unix().ToString(CultureInfo.InvariantCulture)));
            claims.Add(new Claim("nbf", timeNow.Unix().ToString(CultureInfo.InvariantCulture)));
            claims.Add(new Claim("iss", _tokenOptions.Issuer));

            claims.AddRange(userClaims);

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

            var base64 = new byte[Base64.GetMaxEncodedToUtf8Length(signature.Length)];
            Base64.EncodeToUtf8(signature, base64, out _, out _);

            var signedData = new byte[data.Length + 1 + base64.Length];
            Buffer.BlockCopy(data, 0, signedData, 0, data.Length);
            signedData[data.Length] = (byte) '.';
            Buffer.BlockCopy(base64, 0, signedData, data.Length + 1, base64.Length);

            return Encoding.UTF8.GetString(signedData);
        }

        private static async Task<RSA> LoadPrivateKeyAsync()
        {
            var filepath = Environment.GetEnvironmentVariable(Constants.RsaKeyFileVariable);
            if (string.IsNullOrEmpty(filepath))
            {
                throw new InvalidOperationException($"The environment variable '{Constants.RsaKeyFileVariable}' must be set.");
            }

            await using var reader = File.OpenRead(filepath);
            await using var stream = new MemoryStream();
            await reader.CopyToAsync(stream);
            var data = stream.ToArray();

            var cert = new X509Certificate2(data);
            return cert.GetRSAPrivateKey();
        }
    }
}
