using Microsoft.Extensions.Options;
using Shared;
using Shared.Exceptions;
using Shared.Passwords;
using System;
using System.Collections.Generic;
using System.Linq;
using Users.Domain.Dtos;
using Users.Domain.Models;
using Xunit;

namespace Users.Domain.Tests.Models
{
    public class UserTests
    {
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPasswordValidator _passwordValidator;

        public UserTests()
        {
            var options = Options.Create(new PasswordOptions
            {
                Hasher =
                {
                    IterationCount = 1000,
                    KeySize = 128,
                    SaltSize = 64
                }
            });
            _passwordGenerator = new PasswordGenerator(options);
            _passwordHasher = new PasswordHasher(options);
            _passwordValidator = new PasswordValidator(options);
        }

        [Theory]
        [InlineData("John", "Doe", "john@doe.com")]
        [InlineData("Jane", "Doe", "jane@doe.com")]
        public void TestCreate(string firstname, string lastname, string email)
        {
            var dto = new CreateUserDto
            {
                Firstname = firstname,
                Lastname = lastname,
                Email = email
            };
            var (user, password) = User.Create(dto, _passwordHasher, _passwordGenerator);
            Assert.Equal(firstname, user.Firstname);
            Assert.Equal(lastname, user.Lastname);
            Assert.Equal(email, user.Email.ToLowerInvariant());
            Assert.Equal(User.RandomPasswordLength, password.Length);
            Assert.NotNull(user.Roles);
        }

        [Theory]
        [InlineData("", "Doe", "john@doe.com")]
        [InlineData(null, "Doe", "john@doe.com")]
        public void TestCreateWithEmptyFirstname(string firstname, string lastname, string email)
        {
            var dto = new CreateUserDto
            {
                Firstname = firstname,
                Lastname = lastname,
                Email = email
            };

            var ex = Assert.Throws<ValidationException>(() => User.Create(dto, _passwordHasher, _passwordGenerator));
            Assert.Equal(ErrorMessages.UserFirstnameRequired, ex.Message);
        }

        [Fact]
        public void TestCreateWith256CharFirstname()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz ";
            var name = "";

            for (var i = 0; i < 256; i++)
            {
                var rnd = new Random();
                name += chars[rnd.Next(0, chars.Length - 1)];
            }

            var dto = new CreateUserDto
            {
                Firstname = name,
                Lastname = "Doe",
                Email = "john@doe.com"
            };

            var ex = Assert.Throws<ValidationException>(() => User.Create(dto, _passwordHasher, _passwordGenerator));
            Assert.Equal(ErrorMessages.UserFirstnameTooLong, ex.Message);
        }

        [Theory]
        [InlineData("John", "", "john@doe.com")]
        [InlineData("John", null, "john@doe.com")]
        public void TestCreateWithEmptyLastname(string firstname, string lastname, string email)
        {
            var dto = new CreateUserDto
            {
                Firstname = firstname,
                Lastname = lastname,
                Email = email
            };

            var ex = Assert.Throws<ValidationException>(() => User.Create(dto, _passwordHasher, _passwordGenerator));
            Assert.Equal(ErrorMessages.UserLastnameRequired, ex.Message);
        }

        [Fact]
        public void TestCreateWith256CharLastname()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz ";
            var name = "";

            for (var i = 0; i < 256; i++)
            {
                var rnd = new Random();
                name += chars[rnd.Next(0, chars.Length - 1)];
            }

            var dto = new CreateUserDto
            {
                Firstname = "John",
                Lastname = name,
                Email = "john@doe.com"
            };

            var ex = Assert.Throws<ValidationException>(() => User.Create(dto, _passwordHasher, _passwordGenerator));
            Assert.Equal(ErrorMessages.UserLastnameTooLong, ex.Message);
        }

        [Theory]
        [InlineData("John", "Doe", "")]
        [InlineData("John", "Doe", null)]
        public void TestCreateWithEmptyEmail(string firstname, string lastname, string email)
        {
            var dto = new CreateUserDto
            {
                Firstname = firstname,
                Lastname = lastname,
                Email = email
            };

            var ex = Assert.Throws<ValidationException>(() => User.Create(dto, _passwordHasher, _passwordGenerator));
            Assert.Equal(ErrorMessages.UserEmailRequired, ex.Message);
        }

        [Fact]
        public void TestCreateWith256CharEmail()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz ";
            var email = "";

            for (var i = 0; i < 256; i++)
            {
                var rnd = new Random();
                email += chars[rnd.Next(0, chars.Length - 1)];
            }

            var dto = new CreateUserDto
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = email
            };

            var ex = Assert.Throws<ValidationException>(() => User.Create(dto, _passwordHasher, _passwordGenerator));
            Assert.Equal(ErrorMessages.UserEmailTooLong, ex.Message);
        }

        [Theory]
        [InlineData("John", "Doe", "hello@world .com")]
        [InlineData("John", "Doe", "john@doe.internet")]
        [InlineData("John", "Doe", "hello@world")]
        [InlineData("John", "Doe", "johndoe.com")]
        public void TestCreateWithInvalidEmail(string firstname, string lastname, string email)
        {
            var dto = new CreateUserDto
            {
                Firstname = firstname,
                Lastname = lastname,
                Email = email
            };

            var ex = Assert.Throws<ValidationException>(() => User.Create(dto, _passwordHasher, _passwordGenerator));
            Assert.Equal(ErrorMessages.UserEmailInvalid, ex.Message);
        }

        [Theory]
        [InlineData("MyTestPassword1")]
        [InlineData("Hello_World!")]
        public void TestUpdatePassword(string newPassword)
        {
            var (user, currentPassword) = CreateTestUser();
            
            var dto = new ChangePasswordDto
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword,
            };
            
            user.UpdatePassword(dto, _passwordHasher, _passwordValidator);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TestUpdatePasswordWithEmptyNewPassword(string newPassword)
        {
            var (user, currentPassword) = CreateTestUser();

            var dto = new ChangePasswordDto
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword,
            };

            var ex = Assert.Throws<ValidationException>(() => user.UpdatePassword(dto, _passwordHasher, _passwordValidator));
            Assert.Equal(ErrorMessages.UserPasswordRequired, ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TestUpdatePasswordWithEmptyCurrentPassword(string currentPassword)
        {
            var (user, password) = CreateTestUser();

            var dto = new ChangePasswordDto
            {
                CurrentPassword = currentPassword,
                NewPassword = password, // using password as a non-null string
            };

            var ex = Assert.Throws<ValidationException>(() => user.UpdatePassword(dto, _passwordHasher, _passwordValidator));
            Assert.Equal(ErrorMessages.UserConfirmPasswordRequired, ex.Message);
        }

        [Fact]
        public void TestUpdatePasswordWithInvalidCurrentPassword()
        {
            var (user, currentPassword) = CreateTestUser();

            var dto = new ChangePasswordDto
            {
                CurrentPassword = new string(currentPassword.Reverse().ToArray()), // make a new string which doesn't match
                NewPassword = currentPassword // using currentPassword as a non-null string
            };

            var ex = Assert.Throws<ValidationException>(() => user.UpdatePassword(dto, _passwordHasher, _passwordValidator));
            Assert.Equal(ErrorMessages.UserPasswordInvalid, ex.Message);
        }

        [Fact]
        public void TestVerifyPassword()
        {
            var (user, currentPassword) = CreateTestUser();

            Assert.True(user.VerifyPassword(currentPassword, _passwordHasher));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TestVerifyPasswordWithEmptyPassword(string password)
        {
            var (user, _) = CreateTestUser();
            
            Assert.False(user.VerifyPassword(password, _passwordHasher));
        }

        [Fact]
        public void TestVerifyPasswordWithInvalidPassword()
        {
            var (user, currentPassword) = CreateTestUser();
            var invalidPassword = new string(currentPassword.Reverse().ToArray());

            Assert.False(user.VerifyPassword(invalidPassword, _passwordHasher));
        }

        [Theory]
        [InlineData("Jane", "Doe", "jane@doe.com")]
        [InlineData("John", "Smith", "john@smith.com")]
        public void TestUpdate(string firstname, string lastname, string email)
        {
            var (user, _) = CreateTestUser();
            var dto = new UpdateUserDto
            {
                Firstname = firstname,
                Lastname = lastname,
                Email = email
            };

            user.Update(dto);

            Assert.Equal(firstname, user.Firstname);
            Assert.Equal(lastname, user.Lastname);
            Assert.Equal(email, user.Email.ToLowerInvariant());
        }

        private (User user, string password) CreateTestUser(string firstname = "John", string lastname = "Doe",
            string email = "john@doe.com")
        {
            var dto = new CreateUserDto
            {
                Firstname = firstname,
                Lastname = lastname,
                Email = email
            };

            return User.Create(dto, _passwordHasher, _passwordGenerator);
        }

        [Fact]
        public void TestLazyLoadingUsers()
        {
            var roles = new List<UserRole>
            {
                new UserRole("Hello", "World")
            };
            var lazyLoader = new MockLazyLoader(new Dictionary<string, object>
            {
                {"Roles", roles}
            });

            var user = new User(lazyLoader);
            Assert.Equal(roles, user.Roles.ToList());
        }

        [Fact]
        public void TestAddRole()
        {
            var lazyLoader = new MockLazyLoader(new Dictionary<string, object>
            {
                {"Roles", new List<UserRole>()}
            });

            const string userId = "234567";
            const string roleId = "987442";

            var (user, _) = CreateTestUser();
            typeof(User).GetProperty("Id")?.SetValue(user, userId);
            typeof(User).GetProperty("_lazyLoader")?.SetValue(user, lazyLoader);

            user.AddRole(roleId);
            Assert.Contains(user.Roles, x => x.UserId == userId && x.RoleId == roleId);
        }

        [Fact]
        public void TestAddRoleWithDuplicate()
        {
            var lazyLoader = new MockLazyLoader(new Dictionary<string, object>
            {
                {"Roles", new List<UserRole>()},
            });

            const string roleId = "987442";

            var (user, _) = CreateTestUser();
            typeof(User).GetProperty("_lazyLoader")?.SetValue(user, lazyLoader);

            user.AddRole(roleId);
            Assert.Contains(user.Roles, x => x.UserId == user.Id && x.RoleId == roleId);

            var ex = Assert.Throws<ValidationException>(() => user.AddRole(roleId));
            Assert.Equal(ErrorMessages.UserAlreadyAssignedToRole, ex.Message);
        }

        [Fact]
        public void TestRemoveRoleWithNonExistingRole()
        {
            var lazyLoader = new MockLazyLoader(new Dictionary<string, object>
            {
                {"Roles", new List<UserRole>()},
            });
            var (user, _) = CreateTestUser();
            typeof(User).GetProperty("_lazyLoader")?.SetValue(user, lazyLoader);

            const string roleId = "987442";
            var ex = Assert.Throws<ValidationException>(() => user.RemoveRole(roleId));
            Assert.Equal(ErrorMessages.UserNotAssignedToRole, ex.Message);
        }

        [Fact]
        public void TestRemoveRole()
        {
            var lazyLoader = new MockLazyLoader(new Dictionary<string, object>
            {
                {"Roles", new List<UserRole>()},
            });
            var (user, _) = CreateTestUser();
            typeof(User).GetProperty("_lazyLoader")?.SetValue(user, lazyLoader);

            const string roleId = "987442";
            user.AddRole(roleId); // seed role

            user.RemoveRole(roleId);
        }
    }
}
