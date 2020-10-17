using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;

[assembly: InternalsVisibleTo("Shared.Tests")]
namespace Shared.Passwords
{
    internal class PasswordGenerator : IPasswordGenerator
    {
        private readonly PasswordGeneratorOptions _options;

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
        }

        public string Generate(int length)
        {
            var password = new char[length];
            var possibleChars = _options.UppercaseCharacters + _options.LowercaseCharacters + _options.Digits + _options.SpecialCharacters;

            for (var i = 0; i < length; i++)
            {
                var rnd = new Random();
                var character = possibleChars[rnd.Next(0, possibleChars.Length - 1)];

                password[i] = character;
            }

            return new string(password);
        }
    }
}
