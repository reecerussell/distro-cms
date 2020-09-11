using System;

namespace Pages.Domain.Dtos
{
    public class PageDto
    {
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Url { get; set; }
        public bool Active { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }
}
