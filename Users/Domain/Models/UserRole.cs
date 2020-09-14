using Shared.Entity;
using System;

namespace Users.Domain.Models
{
    public class UserRole : Entity
    {
        public string UserId { get; protected set; }
        public string RoleId { get; protected set; }

        public Role Role { get; private set; }
        
        private UserRole()
        {
        }

        public UserRole(string userId, string roleId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (string.IsNullOrEmpty(roleId))
            {
                throw new ArgumentNullException(nameof(roleId));
            }

            UserId = userId;
            RoleId = roleId;
        }
    }
}
