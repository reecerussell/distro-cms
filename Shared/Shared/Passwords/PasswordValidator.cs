using Microsoft.Extensions.Options;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Shared.Tests")]
namespace Shared.Passwords
{
    internal class PasswordValidator : IPasswordValidator
    {
        private readonly PasswordValidationOptions _options;

        public PasswordValidator(IOptions<PasswordOptions> optionsProvider)
        {
            var options = optionsProvider?.Value?.Validation;
            if (options == null)
            {
                throw new InvalidOperationException("PasswordValidatorOptions must be configured before the PasswordValidator is used.");
            }

            _options = options;
        }

        /// <summary>
        /// Validates a given password against a set of preconfigured <see cref="PasswordValidationOptions"/>.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <exception cref="ValidationException">Throws if password is not valid.</exception>
        public void Validate(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ValidationException(ErrorMessages.PasswordRequired);
            }

            if (password.Length < _options.RequiredLength)
            {
                throw new ValidationException(
                    string.Format(ErrorMessages.PasswordTooShort, _options.RequiredLength));
            }

            var hasNonAlphanumeric = false;
            var hasDigit = false;
            var hasLower = false;
            var hasUpper = false;
            var uniqueChars = new List<char>();

            foreach (var c in password)
            {
                if (!hasNonAlphanumeric && !IsLetterOrDigit(c))
                {
                    hasNonAlphanumeric = true;
                }

                if (!hasDigit && IsDigit(c))
                {
                    hasDigit = true;
                }

                if (!hasLower && IsLower(c))
                {
                    hasLower = true;
                }

                if (!hasUpper && IsUpper(c))
                {
                    hasUpper = true;
                }

                if (!uniqueChars.Contains(c))
                {
                    uniqueChars.Add(c);
                }
            }

            if (_options.RequireNonAlphanumeric && !hasNonAlphanumeric)
            {
                throw new ValidationException(ErrorMessages.PasswordRequiresNonAlphanumeric);
            }

            if (_options.RequireDigit && !hasDigit)
            {
                throw new ValidationException(ErrorMessages.PasswordRequiresDigit);
            }

            if (_options.RequireLowercase && !hasLower)
            {
                throw new ValidationException(ErrorMessages.PasswordRequiresLowercase);
            }

            if (_options.RequireUppercase && !hasUpper)
            {
                throw new ValidationException(ErrorMessages.PasswordRequiresUppercase);
            }

            if (uniqueChars.Count < _options.RequiredUniqueChars)
            {
                throw new ValidationException(
                    string.Format(ErrorMessages.PasswordRequiresUniqueChars, _options.RequiredUniqueChars));
            }
        }

        /// <summary>
        /// Returns a flag indicating whether the supplied character
        /// is a digit - true if the character is a digit, otherwise false.
        /// </summary>
        /// <param name="c">An ASCII character</param>
        /// <returns>A flag determining if <paramref name="c"/> is a digit.</returns>
        internal static bool IsDigit(char c) => c >= '0' && c <= '9';

        /// <summary>
        /// Returns a flag indicating whether the supplied character
        /// is lowercase - true if the character is a lowercase, otherwise false.
        /// </summary>
        /// <param name="c">An ASCII character</param>
        /// <returns>A flag determining if <paramref name="c"/> is lowercase.</returns>
        internal static bool IsLower(char c) => c >= 'a' && c <= 'z';

        /// <summary>
        /// Returns a flag indicating whether the supplied character
        /// is uppercase - true if the character is a uppercase, otherwise false.
        /// </summary>
        /// <param name="c">An ASCII character</param>
        /// <returns>A flag determining if <paramref name="c"/> is uppercase.</returns>
        internal static bool IsUpper(char c) => c >= 'A' && c <= 'Z';

        /// <summary>
        /// Returns a flag indicating whether the supplied character is
        /// an ASCII letter or digit - true if the character is an ASCII letter or digit,
        /// otherwise false.
        /// </summary>
        /// <param name="c">An ASCII character</param>
        /// <returns>A flag determining if <paramref name="c"/> is an ASCII letter or digit.</returns>
        internal static bool IsLetterOrDigit(char c) => IsDigit(c) || IsLower(c) || IsUpper(c);
    }
}
