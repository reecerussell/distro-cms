using System;
using Users.Domain.Models;
using Xunit;

namespace Users.Domain.Tests.Models
{
    public class UserRoleTests
    {
        [Fact]
        public void TestConstructorWithNullUserId()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new UserRole(null, "323"));
            Assert.Contains("userId", ex.Message);
        }

        [Fact]
        public void TestConstructorWithNullRoleId()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new UserRole("323", null));
            Assert.Contains("roleId", ex.Message);
        }

        [Theory]
        [InlineData("454", "239")]
        [InlineData("34", "he34")]
        [InlineData("23784", "43")]
        public void TestConstructor(string userId, string roleId)
        {
            var userRole = new UserRole(userId, roleId);
            Assert.Equal(userId, userRole.UserId);
            Assert.Equal(roleId, userRole.RoleId);
            Assert.Null(userRole.Role);
        }
    }
}
