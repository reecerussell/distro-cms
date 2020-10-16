using Microsoft.Extensions.Options;
using Shared.Exceptions;
using Shared.Passwords;
using System;
using Xunit;

namespace Shared.Tests.Passwords
{
    public class HasherTests
    {
        [Fact]
        public void TestConstructorWithNullOptions()
        {
            Assert.Throws<NullReferenceException>(() => new PasswordHasher(null));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        public void TestConstructorWithInvalidIterationCount(int iterationCount)
        {
            var passwordOptions = new PasswordOptions {Hasher = {IterationCount = iterationCount}};

            Assert.Throws<InvalidIterationCountException>(() => new PasswordHasher(Options.Create(passwordOptions)));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        [InlineData(23)]
        [InlineData(183)]
        public void TestConstructorWithInvalidKeySize(int keySize)
        {
            var passwordOptions = new PasswordOptions 
            { 
                Hasher = {
                    IterationCount = 1000, // Valid iteration count
                    KeySize = keySize
                }
            };

            Assert.Throws<InvalidKeySizeException>(() => new PasswordHasher(Options.Create(passwordOptions)));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        [InlineData(23)]
        [InlineData(183)]
        public void TestConstructorWithInvalidSaltSize(int saltSize)
        {
            var passwordOptions = new PasswordOptions
            {
                Hasher = {
                    IterationCount = 1000, // Valid iteration count
                    KeySize = 256, // Valid key size
                    SaltSize = saltSize
                }
            };

            Assert.Throws<InvalidSaltSizeException>(() => new PasswordHasher(Options.Create(passwordOptions)));
        }

        [Theory]
        [InlineData(100, 256, 128)]
        [InlineData(1000, 128, 64)]
        [InlineData(267, 64, 32)]
        public void TestConstructor(int iterationCount, int keySize, int saltSize)
        {
            var passwordOptions = new PasswordOptions
            {
                Hasher = {
                    IterationCount = iterationCount,
                    KeySize = keySize,
                    SaltSize = saltSize
                }
            };

            var hasher = new PasswordHasher(Options.Create(passwordOptions));
            Assert.NotNull(hasher);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TestHashWithNullPassword(string password)
        {
            var hasher = CreateValidHasher();

            Assert.Throws<ArgumentNullException>(() => hasher.Hash(password));
        }

        [Theory]
        [InlineData("MyPassword1")]
        [InlineData("54b£$ gtr3")]
        public void TestHash(string password)
        {
            var hasher = CreateValidHasher();
            var hash = hasher.Hash(password);
            Assert.NotNull(hash);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TestVerifyWithNullPassword(string password)
        {
            var hasher = CreateValidHasher();

            var ex = Assert.Throws<ArgumentNullException>(() => hasher.Verify(password, "t32i4db="));
            Assert.Contains("pwd", ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TestVerifyWithNullHash(string hash)
        {
            var hasher = CreateValidHasher();

            var ex = Assert.Throws<ArgumentNullException>(() => hasher.Verify("myPassword1", hash));
            Assert.Contains("base64Hash", ex.Message);
        }

        [Theory]
        [InlineData("7632gdds=")]
        [InlineData("anf4r")]
        public void TestVerifyWithInvalidBase64Hash(string base64Hash)
        {
            var hasher = CreateValidHasher();

            var valid = hasher.Verify("myTestPassword", base64Hash);
            Assert.False(valid);
        }

        [Fact]
        public void TestVerifyWithInvalidFormatMarker()
        {
            const string testPassword = "myTestPassword";
            var hasher = CreateValidHasher();
            var hash = hasher.Hash(testPassword);
            var hashBytes = Convert.FromBase64String(hash);
            hashBytes[0] = 0x02;
            hash = Convert.ToBase64String(hashBytes);

            var valid = hasher.Verify(testPassword, hash);
            Assert.False(valid);
        }

        [Theory]
        [InlineData(34)]
        [InlineData(78)]
        public void TestVerifyWithInvalidSaltSize(int hashSaltSize)
        {
            const string testPassword = "myTestPassword";
            const int saltSize = 64;

            var hasher = CreateValidHasher(saltSize: saltSize);
            var hash = hasher.Hash(testPassword);
            var hashBytes = Convert.FromBase64String(hash);

            // Update salt size definition
            WriteHeader(hashBytes, 9, (uint)hashSaltSize);
            hash = Convert.ToBase64String(hashBytes);

            var valid = hasher.Verify(testPassword, hash);
            Assert.False(valid);
        }

        [Fact]
        public void TestVerifyWithInvalidKeySize()
        {
            const string testPassword = "myTestPassword";

            var hasher = CreateValidHasher();
            var hash = hasher.Hash(testPassword);
            var hashBytes = Convert.FromBase64String(hash);
            hash = Convert.ToBase64String(hashBytes[..^3]); // Change size of hash 

            var valid = hasher.Verify(testPassword, hash);
            Assert.False(valid);
        }

        [Theory]
        [InlineData(238)]
        [InlineData(1000)]
        public void TestVerifyWithInvalidIterationCount(int iterationCount)
        {
            const string testPassword = "myTestPassword";
            var hasher = CreateValidHasher(iterationCount: 100);
            var hash = hasher.Hash(testPassword);
            var hashBytes = Convert.FromBase64String(hash);

            // Update iteration count definition
            WriteHeader(hashBytes, 5, (uint)iterationCount);
            hash = Convert.ToBase64String(hashBytes);

            var valid = hasher.Verify(testPassword, hash);
            Assert.False(valid);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(67)]
        public void TestVerifyWithInvalidAlg(int alg)
        {
            const string testPassword = "myTestPassword";

            var hasher = CreateValidHasher();
            var hash = hasher.Hash(testPassword);
            var hashBytes = Convert.FromBase64String(hash);

            // Update iteration count definition
            WriteHeader(hashBytes, 1, (uint)alg);
            hash = Convert.ToBase64String(hashBytes);

            var valid = hasher.Verify(testPassword, hash);
            Assert.False(valid);
        }

        [Theory]
        [InlineData("MyFirstPassword")]
        [InlineData("password")]
        [InlineData("eg3ydhbnewr")]
        public void TestVerify(string password)
        {
            var hasher = CreateValidHasher();
            var hash = hasher.Hash(password);

            var valid = hasher.Verify(password, hash);
            Assert.True(valid);
        }

        private static IPasswordHasher CreateValidHasher(int iterationCount = 10, int keySize = 128, int saltSize = 64)
        {
            var passwordOptions = new PasswordOptions
            {
                Hasher = {
                    IterationCount = iterationCount,
                    KeySize = keySize,
                    SaltSize = saltSize
                }
            };

            return new PasswordHasher(Options.Create(passwordOptions));
        }

        private static void WriteHeader(byte[] buf, int offset, uint value)
        {
            buf[offset + 0] = (byte)(value >> 24);
            buf[offset + 1] = (byte)(value >> 16);
            buf[offset + 2] = (byte)(value >> 8);
            buf[offset + 3] = (byte)(value >> 0);
        }
    }
}
