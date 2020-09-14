using System;

namespace Users.Domain.Dtos
{
    public class RoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
