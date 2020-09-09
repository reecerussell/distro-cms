using System;

namespace Shared.Entity
{
    public class Entity
    {
        public string Id { get; internal set; }
        public DateTime DateCreated { get; internal set; }
        public DateTime? DateUpdated { get; internal set; }

        protected Entity()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.UtcNow;
        }
    }
}
