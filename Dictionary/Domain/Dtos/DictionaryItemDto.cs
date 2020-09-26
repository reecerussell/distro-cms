using System;

namespace Dictionary.Domain.Dtos
{
    public class DictionaryItemDto
    {
        public string Id { get; set; }
        public string CultureId { get; set; }
        public string Key { get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
