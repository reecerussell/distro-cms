using System;

namespace Dictionary.Domain.Dtos
{
    public class SupportedCultureDto
    {
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsDefault { get; set; }
    }
}
