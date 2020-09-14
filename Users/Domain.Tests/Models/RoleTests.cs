using System;
using Shared;
using Shared.Exceptions;
using Users.Domain.Dtos;
using Users.Domain.Models;
using Xunit;

namespace Users.Domain.Tests.Models
{
    public class RoleTests
    {
        [Fact]
        public void TestCreateWithNullDto()
        {
            Assert.Throws<ArgumentNullException>(() => { Role.Create(null); });
        }

        [Theory]
        [InlineData("MyRole")]
        [InlineData("Test Role")]
        [InlineData("Hello World")]
        public void TestCreate(string name)
        {
            var dto = new CreateRoleDto
            {
                Name = name
            };

            var role = Role.Create(dto);
            Assert.NotNull(role);
            Assert.NotEmpty(role.Id);
            Assert.NotEqual(default, role.DateCreated);
            Assert.Null(role.DateUpdated);
            Assert.Equal(name, role.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void TestUpdateNameWithEmptyValue(string value)
        {
            var role = Role.Create(new CreateRoleDto { Name = "TestName" });

            var ex = Assert.Throws<ValidationException>(() => { role.UpdateName(value); });
            Assert.Equal(ErrorMessages.RoleNameRequired, ex.Message);
        }

        [Theory]
        [InlineData("Hello this is my super long role name. What do you think?")]
        [InlineData("xeCoQ6BtFEYn5DYLqg9wxeCoQ6BtFEYn5DYLqg9wxeCoQ6BtFEYn5DYLqg9w")]
        public void TestUpdateNameWithLongName(string value)
        {
            var role = Role.Create(new CreateRoleDto { Name = "TestName" });

            var ex = Assert.Throws<ValidationException>(() => { role.UpdateName(value); });
            Assert.Equal(ErrorMessages.RoleNameTooLong, ex.Message);
        }
    }
}
