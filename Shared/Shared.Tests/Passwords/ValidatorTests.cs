using System;
using Microsoft.Extensions.Options;
using Shared.Exceptions;
using Shared.Passwords;
using Xunit;

namespace Shared.Tests.Passwords
{
    public class ValidatorTests
    {
        [Fact]
        public void TestConstructorWithNullOptions()
        {
            Assert.Throws<InvalidOperationException>(() => new PasswordValidator(null));
            Assert.Throws<InvalidOperationException>(() =>
            {
                var options = Options.Create<PasswordOptions>(null);
                _ = new PasswordValidator(options);
            });
            Assert.Throws<InvalidOperationException>(() =>
            {
                var options = Options.Create(new PasswordOptions{Validation = null});
                _ = new PasswordValidator(options);
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TestValidateWithEmptyOrNullPassword(string password)
        {
            var validator = new PasswordValidator(Options.Create(new PasswordOptions()));
            var ex = Assert.Throws<ValidationException>(() => validator.Validate(password));
            Assert.Equal(ErrorMessages.PasswordRequired, ex.Message);
        }

        [Theory]
        [InlineData("3849")]
        [InlineData("0jnvs")]
        [InlineData("Pass12")]
        public void TestValidateWithPasswordShorterThan10(string password)
        {
            var options = new PasswordOptions
            {
                Validation =
                {
                    RequiredLength = 10
                }
            };
            var validator = new PasswordValidator(Options.Create(options));
            var ex = Assert.Throws<ValidationException>(() => validator.Validate(password));
            Assert.Equal(
                string.Format(ErrorMessages.PasswordTooShort, options.Validation.RequiredLength), 
                ex.Message);
        }

        [Theory]
        [InlineData("aBc3inds")]
        [InlineData("3nedd9nc")]
        public void TestValidateWhereNonAlphaNumericIsRequired(string password)
        {
            var options = new PasswordOptions
            {
                Validation =
                {
                    RequiredLength = 8,
                    RequireNonAlphanumeric = true
                }
            };
            var validator = new PasswordValidator(Options.Create(options));
            var ex = Assert.Throws<ValidationException>(() => validator.Validate(password));
            Assert.Equal(
                ErrorMessages.PasswordRequiresNonAlphanumeric,
                ex.Message);
        }

        [Theory]
        [InlineData("aBc3i+nds")]
        [InlineData("3ned-d9nc")]
        public void TestValidateWhereNonAlphaNumericIsRequiredWithAlphaNumeric(string password)
        {
            var options = new PasswordOptions
            {
                Validation =
                {
                    RequiredLength = 8,
                    RequireNonAlphanumeric = true
                }
            };
            var validator = new PasswordValidator(Options.Create(options));
            validator.Validate(password);
        }

        [Theory]
        [InlineData("aBcf@nds")]
        [InlineData("!nedd-nc")]
        public void TestValidateWhereDigitIsRequired(string password)
        {
            var options = new PasswordOptions
            {
                Validation =
                {
                    RequiredLength = 8,
                    RequireDigit = true
                }
            };
            var validator = new PasswordValidator(Options.Create(options));
            var ex = Assert.Throws<ValidationException>(() => validator.Validate(password));
            Assert.Equal(
                ErrorMessages.PasswordRequiresDigit,
                ex.Message);
        }

        [Theory]
        [InlineData("aBc3i+nds")]
        [InlineData("3ned-d9nc")]
        public void TestValidateDigitIsRequiredWithDigit(string password)
        {
            var options = new PasswordOptions
            {
                Validation =
                {
                    RequiredLength = 8,
                    RequireDigit = true
                }
            };
            var validator = new PasswordValidator(Options.Create(options));
            validator.Validate(password);
        }

        [Theory]
        [InlineData("ABCF@123")]
        [InlineData("!AKEM@12")]
        public void TestValidateWhereLowercaseIsRequired(string password)
        {
            var options = new PasswordOptions
            {
                Validation =
                {
                    RequiredLength = 8,
                    RequireLowercase = true
                }
            };
            var validator = new PasswordValidator(Options.Create(options));
            var ex = Assert.Throws<ValidationException>(() => validator.Validate(password));
            Assert.Equal(
                ErrorMessages.PasswordRequiresLowercase,
                ex.Message);
        }

        [Theory]
        [InlineData("aBc3i+nds")]
        [InlineData("3ned-d9nc")]
        public void TestValidateWhereLowercaseIsRequiredWithLowercase(string password)
        {
            var options = new PasswordOptions
            {
                Validation =
                {
                    RequiredLength = 8,
                    RequireLowercase = true
                }
            };
            var validator = new PasswordValidator(Options.Create(options));
            validator.Validate(password);
        }

        [Theory]
        [InlineData("abcf@123")]
        [InlineData("!akem@12")]
        public void TestValidateWhereUppercaseIsRequired(string password)
        {
            var options = new PasswordOptions
            {
                Validation =
                {
                    RequiredLength = 8,
                    RequireUppercase = true
                }
            };
            var validator = new PasswordValidator(Options.Create(options));
            var ex = Assert.Throws<ValidationException>(() => validator.Validate(password));
            Assert.Equal(
                ErrorMessages.PasswordRequiresUppercase,
                ex.Message);
        }

        [Theory]
        [InlineData("aBc3i+nds")]
        [InlineData("3NEd-d9nc")]
        public void TestValidateWhereUppercaseRequiredWithUppercase(string password)
        {
            var options = new PasswordOptions
            {
                Validation =
                {
                    RequiredLength = 8,
                    RequireUppercase = true
                }
            };
            var validator = new PasswordValidator(Options.Create(options));
            validator.Validate(password);
        }

        [Theory]
        [InlineData("Password")]
        [InlineData("HelloWorld")]
        public void TestValidateWhere8UniqueCharsAreRequired(string password)
        {
            var options = new PasswordOptions
            {
                Validation =
                {
                    RequiredLength = 8,
                    RequireDigit = false,
                    RequiredUniqueChars = 8
                }
            };
            var validator = new PasswordValidator(Options.Create(options));
            var ex = Assert.Throws<ValidationException>(() => validator.Validate(password));
            Assert.Equal(
                string.Format(ErrorMessages.PasswordRequiresUniqueChars, options.Validation.RequiredUniqueChars),
                ex.Message);
        }

        [Theory]
        [InlineData('0')]
        [InlineData('1')]
        [InlineData('2')]
        [InlineData('3')]
        [InlineData('4')]
        [InlineData('5')]
        [InlineData('6')]
        [InlineData('7')]
        [InlineData('8')]
        [InlineData('9')]
        public void TestIsDigit(char c)
        {
            Assert.True(PasswordValidator.IsDigit(c));
        }

        [Theory]
        [InlineData('d')]
        [InlineData('H')]
        [InlineData('.')]
        [InlineData('-')]
        public void TestIsNotDigit(char c)
        {
            Assert.False(PasswordValidator.IsDigit(c));
        }

        [Theory]
        [InlineData('a')]
        [InlineData('b')]
        [InlineData('c')]
        [InlineData('d')]
        [InlineData('e')]
        [InlineData('f')]
        [InlineData('g')]
        [InlineData('h')]
        [InlineData('i')]
        [InlineData('j')]
        [InlineData('k')]
        [InlineData('l')]
        [InlineData('m')]
        [InlineData('n')]
        [InlineData('o')]
        [InlineData('p')]
        [InlineData('q')]
        [InlineData('r')]
        [InlineData('s')]
        [InlineData('t')]
        [InlineData('u')]
        [InlineData('v')]
        [InlineData('w')]
        [InlineData('x')]
        [InlineData('y')]
        [InlineData('z')]
        public void TestIsLower(char c)
        {
            Assert.True(PasswordValidator.IsLower(c));
        }

        [Theory]
        [InlineData('7')]
        [InlineData('-')]
        [InlineData('U')]
        [InlineData('L')]
        [InlineData('F')]
        public void TestIsNotLower(char c)
        {
            Assert.False(PasswordValidator.IsLower(c));
        }

        [Theory]
        [InlineData('A')]
        [InlineData('B')]
        [InlineData('C')]
        [InlineData('D')]
        [InlineData('E')]
        [InlineData('F')]
        [InlineData('G')]
        [InlineData('H')]
        [InlineData('I')]
        [InlineData('J')]
        [InlineData('K')]
        [InlineData('L')]
        [InlineData('M')]
        [InlineData('N')]
        [InlineData('O')]
        [InlineData('P')]
        [InlineData('Q')]
        [InlineData('R')]
        [InlineData('S')]
        [InlineData('T')]
        [InlineData('U')]
        [InlineData('V')]
        [InlineData('W')]
        [InlineData('X')]
        [InlineData('Y')]
        [InlineData('Z')]
        public void TestIsUpper(char c)
        {
            Assert.True(PasswordValidator.IsUpper(c));
        }

        [Theory]
        [InlineData('7')]
        [InlineData('-')]
        [InlineData('u')]
        [InlineData('l')]
        [InlineData('f')]
        public void TestIsNotUpper(char c)
        {
            Assert.False(PasswordValidator.IsUpper(c));
        }

        [Theory]
        [InlineData('a')]
        [InlineData('3')]
        [InlineData('D')]
        public void TestIsLetterOrDigit(char c)
        {
            Assert.True(PasswordValidator.IsLetterOrDigit(c));
        }

        [Theory]
        [InlineData('-')]
        [InlineData('.')]
        [InlineData('£')]
        public void TestIsNotLetterOrDigit(char c)
        {
            Assert.False(PasswordValidator.IsLetterOrDigit(c));
        }
    }
}
