using Microsoft.Extensions.Options;
using Shared.Passwords;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Shared.Tests.Passwords
{
    public class GeneratorTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public GeneratorTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestConstructorWithNullOptions()
        {
            Assert.Throws<ArgumentNullException>(() => new PasswordGenerator(null));
            Assert.Throws<ArgumentNullException>(() =>
            {
                var options = Options.Create<PasswordOptions>(null);
                _ = new PasswordGenerator(options);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var options = Options.Create(new PasswordOptions {Generator = null});
                _ = new PasswordGenerator(options);
            });
        }

        [Fact]
        public void TestConstructorWithNullLowercaseChars()
        {
            var options = Options.Create(new PasswordOptions {Generator = {LowercaseCharacters = null}});

            var ex = Assert.Throws<ArgumentNullException>(() => new PasswordGenerator(options));
            Assert.Contains("LowercaseCharacters", ex.Message);
        }

        [Fact]
        public void TestConstructorWithNullUppercaseChars()
        {
            var options = Options.Create(new PasswordOptions { Generator = { UppercaseCharacters = null } });

            var ex = Assert.Throws<ArgumentNullException>(() => new PasswordGenerator(options));
            Assert.Contains("UppercaseCharacters", ex.Message);
        }

        [Fact]
        public void TestConstructorWithNullDigits()
        {
            var options = Options.Create(new PasswordOptions { Generator = { Digits = null } });

            var ex = Assert.Throws<ArgumentNullException>(() => new PasswordGenerator(options));
            Assert.Contains("Digits", ex.Message);
        }

        [Fact]
        public void TestConstructorWithNullSpecialChars()
        {
            var options = Options.Create(new PasswordOptions { Generator = { SpecialCharacters = null } });

            var ex = Assert.Throws<ArgumentNullException>(() => new PasswordGenerator(options));
            Assert.Contains("SpecialCharacters", ex.Message);
        }

        [Theory]
        [InlineData(12)]
        [InlineData(32)]
        public void TestGenerate(int length)
        {
            var generator = new PasswordGenerator(Options.Create(new PasswordOptions()));
            var password = generator.Generate(length);
            Assert.Equal(length, password.Length);

            _testOutputHelper.WriteLine(password);
        }
    }
}
