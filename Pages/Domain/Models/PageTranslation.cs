using System;
using System.Globalization;
using Shared;
using Shared.Entity;
using Shared.Exceptions;

namespace Domain.Models
{
    public class PageTranslation : Entity
    {
        public string PageId { get; protected set; }
        public string CultureName { get; protected set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Content { get; private set; }

        private PageTranslation()
        {
        }

        private PageTranslation(string pageId, CultureInfo culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            if (string.IsNullOrEmpty(pageId))
            {
                throw new ArgumentNullException(nameof(pageId));
            }

            PageId = pageId;
            CultureName = culture.Name;
        }

        internal void UpdateTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ValidationException(ErrorMessages.PageTitleRequired);
            }

            if (title.Length > 255)
            {
                throw new ValidationException(ErrorMessages.PageTitleTooLong);
            }

            Title = title;
        }

        internal void UpdateDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new ValidationException(ErrorMessages.PageDescriptionRequired);
            }

            if (description.Length > 255)
            {
                throw new ValidationException(ErrorMessages.PageDescriptionTooLong);
            }

            Description = description;
        }

        internal void UpdateContent(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new ValidationException(ErrorMessages.PageContentRequired);
            }

            var illegalTags = new[] { "script", "link", "body", "html", "head" };

            foreach (var tag in illegalTags)
            {
                if (content.Contains("<" + tag))
                {
                    throw new ValidationException(
                        string.Format(ErrorMessages.PageContentTagNotAllowed, tag));
                }
            }

            Content = content;
        }

        internal static PageTranslation Create(string pageId, CultureInfo culture, string title, string description, string content)
        {
            var translation = new PageTranslation(pageId, culture);
            translation.UpdateTitle(title);
            translation.UpdateDescription(description);
            translation.UpdateContent(content);

            return translation;
        }
    }
}
