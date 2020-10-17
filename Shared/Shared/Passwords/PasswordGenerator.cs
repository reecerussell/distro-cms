using Microsoft.Extensions.Options;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

[assembly: InternalsVisibleTo("Shared.Tests")]
namespace Shared.Passwords
{
    internal class PasswordGenerator : IPasswordGenerator
    {
        private readonly PasswordGeneratorOptions _options;
        private readonly RandomNumberGenerator _rng;

        public PasswordGenerator(IOptions<PasswordOptions> options)
        {
            var generatorOptions = options?.Value?.Generator;
            if (generatorOptions == null)
            {
                throw new ArgumentNullException(nameof(options), "Generator Options have not been configured.");
            }

            if (generatorOptions.LowercaseCharacters == null)
            {
                throw new ArgumentNullException(nameof(generatorOptions.LowercaseCharacters));
            }

            if (generatorOptions.UppercaseCharacters == null)
            {
                throw new ArgumentNullException(nameof(generatorOptions.UppercaseCharacters));
            }

            if (generatorOptions.Digits == null)
            {
                throw new ArgumentNullException(nameof(generatorOptions.Digits));
            }

            if (generatorOptions.SpecialCharacters == null)
            {
                throw new ArgumentNullException(nameof(generatorOptions.SpecialCharacters));
            }

            _options = generatorOptions;
            _rng = new RNGCryptoServiceProvider();
        }

        public string Generate(int length)
        {
            var password = new char[length];
            var possibleChars = _options.UppercaseCharacters + _options.LowercaseCharacters + _options.Digits + _options.SpecialCharacters;

            for (var i = 0; i < length; i++)
            {
                var randIndex = GetRandomIndex(0, possibleChars.Length - 1);
                password[i] = possibleChars[randIndex];
            }

            return new string(password);
        }

        // https://github.com/prjseal/PasswordGenerator/blob/f86da8e021c3f0f47b5a120c404e1df27106ca99/PasswordGenerator/Password.cs#L183
        internal int GetRandomIndex(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be greater than max");
            }

            var data = new byte[sizeof(int)];
            _rng.GetBytes(data);
            var randomIndex = BitConverter.ToInt32(data, 0);

            return (int)Math.Floor((double)(min + Math.Abs(randomIndex % (max - min))));
        }
    }
}
