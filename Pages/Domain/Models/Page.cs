using Microsoft.EntityFrameworkCore.Infrastructure;
using Pages.Domain.Dtos;
using Shared;
using Shared.Entity;
using Shared.Exceptions;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Pages.Domain.Models
{
    public class Page : Aggregate
    {
        public string Url { get; private set; }
        public bool Active { get; private set; }

        private List<PageTranslation> _translations;
        public IReadOnlyList<PageTranslation> Translations
        {
            get => _lazyLoader.Load(this, ref _translations);
            protected set => _translations = (List<PageTranslation>) value;
        }

        private readonly ILazyLoader _lazyLoader;
        
        private Page(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        private Page()
        {
            Translations = new List<PageTranslation>();
        }

        internal void UpdateUrl(string url)
        {
            if (url.Length > 255)
            {
                throw new ValidationException(ErrorMessages.PageUrlTooLong);
            }

            const string allowedChars = "abcdefghijklmnopqrstuvwxyz1234567890-";
            url = url.ToLower();

            foreach (var character in url)
            {
                if (!allowedChars.Contains(character))
                {
                    throw new ValidationException(
                        string.Format(ErrorMessages.PageUrlCharacterNotAllowed, character));
                }
            }

            Url = url;
        }

        public void Activate()
        {
            if (Active)
            {
                throw new ValidationException(ErrorMessages.PageAlreadyActive);
            }

            Active = true;
        }

        public void Deactivate()
        {
            if (!Active)
            {
                throw new ValidationException(ErrorMessages.PageAlreadyInactive);
            }

            Active = false;
        }

        internal void UpdateContent(CultureInfo culture, string title, string description, string content)
        {
            var translation = Translations.SingleOrDefault(x => x.CultureName == culture.Name);
            if (translation == null)
            {
                translation = PageTranslation.Create(Id, culture, title, description, content);
                _translations.Add(translation);
                return;
            }

            translation.UpdateTitle(title);
            translation.UpdateDescription(description);
            translation.UpdateContent(content);
        }

        public void Update(UpdatePageDto dto, CultureInfo culture)
        {
            UpdateContent(culture, dto.Title, dto.Description, dto.Content);
            UpdateUrl(dto.Url);
        }

        public static Page Create(CreatePageDto dto, CultureInfo culture)
        {
            var page = new Page();
            page.UpdateContent(culture, dto.Title, dto.Description, dto.Content);
            page.UpdateUrl(dto.Url);

            return page;
        }
    }
}
