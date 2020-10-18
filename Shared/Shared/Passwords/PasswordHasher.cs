using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Shared.Exceptions;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

[assembly: InternalsVisibleTo("Shared.Tests")]
[assembly: InternalsVisibleTo("Users.Domain.Tests")]
namespace Shared.Passwords
{
    /// <summary>
    /// Similar to the password hasher from Identity, but only containing the hashing logic
    /// for a single approach.
    /// </summary>
    internal class PasswordHasher : IPasswordHasher
    {
        internal const byte FormatMarker = 0x01;
        private const KeyDerivationPrf Algorithm = KeyDerivationPrf.HMACSHA256;

        private readonly RandomNumberGenerator _rng;
        private readonly PasswordHasherOptions _options;

        public PasswordHasher(IOptions<PasswordOptions> optionsProvider)
        {
            var options = optionsProvider?.Value?.Hasher;
            if (options == null)
            {
                throw new NullReferenceException("PasswordHasherOptions must be configured before the PasswordService is used.");
            }

            if (options.IterationCount < 1)
            {
                throw new InvalidIterationCountException();
            }

            if (options.KeySize % 8 != 0 || options.KeySize / 8 < 1)
            {
                throw new InvalidKeySizeException();
            }

            if (options.SaltSize % 8 != 0 || options.SaltSize / 8 < 1)
            {
                throw new InvalidSaltSizeException();
            }

            _rng = new RNGCryptoServiceProvider();
            _options = options;
        }

        public string Hash(string pwd)
        {
            if (string.IsNullOrEmpty(pwd))
            {
                throw new ArgumentNullException(nameof(pwd));
            }

            var salt = new byte[_options.SaltSize];
            _rng.GetBytes(salt);
            var subKey = KeyDerivation.Pbkdf2(pwd, salt, Algorithm, _options.IterationCount, _options.KeySize);

            var output = new byte[13 + _options.SaltSize + _options.KeySize];
            output[0] = FormatMarker;

            WriteHeader(output, 1, (uint)Algorithm);
            WriteHeader(output, 5, (uint)_options.IterationCount);
            WriteHeader(output, 9, (uint)_options.SaltSize);

            Buffer.BlockCopy(salt, 0, output, 13, salt.Length);
            Buffer.BlockCopy(subKey, 0, output, 13 + salt.Length, subKey.Length);

            return Convert.ToBase64String(output);

            static void WriteHeader(byte[] buf, int offset, uint value)
            {
                buf[offset + 0] = (byte)(value >> 24);
                buf[offset + 1] = (byte)(value >> 16);
                buf[offset + 2] = (byte)(value >> 8);
                buf[offset + 3] = (byte)(value >> 0);
            }
        }

        public bool Verify(string pwd, string base64Hash)
        {
            if (string.IsNullOrEmpty(pwd))
            {
                throw new ArgumentNullException(nameof(pwd));
            }

            if (string.IsNullOrEmpty(base64Hash))
            {
                throw new ArgumentNullException(nameof(base64Hash));
            }

            try
            {
                var rawHash = Convert.FromBase64String(base64Hash);
                if (rawHash[0] != FormatMarker)
                {
                    return false;
                }

                var (alg, iterationCount, saltSize) = ScanHeader(rawHash);
                if (saltSize < _options.SaltSize)
                {
                    return false;
                }

                var salt = new byte[saltSize];
                Buffer.BlockCopy(rawHash, 13, salt, 0, saltSize);

                var subKeyLength = rawHash.Length - 13 - saltSize;
                if (subKeyLength < _options.KeySize)
                {
                    return false;
                }

                var expected = new byte[subKeyLength];
                Buffer.BlockCopy(rawHash, 13 + saltSize, expected, 0, subKeyLength);
                var actual = KeyDerivation.Pbkdf2(pwd, salt, alg, iterationCount, subKeyLength);

                return CryptographicOperations.FixedTimeEquals(actual, expected);
            }
            catch
            {
                // This shouldn't happen, unless the given hash was not originally hashing using this
                // service, i.e. the hash is in an invalid format or hashed using a third-party function.

                return false;
            }

            (KeyDerivationPrf Algorithm, int IterationCount, int SaltSize) ScanHeader(byte[] buf)
            {
                var alg = Algorithm;
                var iterationCount = _options.IterationCount;
                var saltSize = _options.SaltSize;

                for (var i = 1; i < 13; i += 4)
                {
                    var v = (((uint)buf[i]) << 24) |
                            ((uint)(buf[i + 1]) << 16) |
                            (((uint)buf[i + 2]) << 8) |
                            ((uint)buf[i + 3]);

                    switch (i)
                    {
                        case 1:
                            alg = (KeyDerivationPrf)v;
                            break;
                        case 5:
                            iterationCount = (int)v;
                            break;
                        case 9:
                            saltSize = (int)v;
                            break;
                    }
                }

                return (alg, iterationCount, saltSize);
            }
        }
    }
}