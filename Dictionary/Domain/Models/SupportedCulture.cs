using System;
using System.Collections.Generic;
using Dictionary.Domain.Dtos;
using Shared;
using Shared.Entity;
using Shared.Exceptions;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Dictionary.Domain.Models
{
    public class SupportedCulture : Aggregate
    {
        public string Name { get; protected set; }
        public string DisplayName { get; private set; }
        public bool IsDefault { get; private set; }

        private List<DictionaryItem> _dictionaryItems;

        public IReadOnlyList<DictionaryItem> DictionaryItems
        {
            get => _lazyLoader.Load(this, ref _dictionaryItems);
            set => _dictionaryItems = (List<DictionaryItem>) value;
        }

        private readonly ILazyLoader _lazyLoader;

        private SupportedCulture(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        private SupportedCulture(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ValidationException(ErrorMessages.SupportedCultureNameRequired);
            }

            if (name.Length > 14)
            {
                throw new ValidationException(ErrorMessages.SupportedCultureNameTooLong);
            }

            try
            {
                var cultureInfo = CultureInfo.CreateSpecificCulture(name);
                Name = cultureInfo.Name;
                DisplayName = cultureInfo.DisplayName;
            }
            catch (CultureNotFoundException)
            {
                throw new ValidationException(ErrorMessages.SupportedCultureUnrecognisedCulture);
            }

            DictionaryItems = new List<DictionaryItem>();
        }

        public void SetAsDefault()
        {
            IsDefault = true;
        }

        public void SetAsNotDefault()
        {
            IsDefault = false;
        }

        private void AddItems(IReadOnlyList<DictionaryItem> items)
        {
            if (items == null)
            {
                return;
            }

            foreach (var item in items)
            {
                var dto = new CreateDictionaryItem
                {
                    Key = item.Key,
                    DisplayName = item.DisplayName,
                    Value = item.Value
                };

                _dictionaryItems.Add(DictionaryItem.Create(dto, this));
            }
        }

        public static SupportedCulture Create(CreateSupportedCultureDto dto)
        {
            return new SupportedCulture(dto.Name);
        }

        public static SupportedCulture Create(CreateSupportedCultureDto dto, IReadOnlyList<DictionaryItem> itemsToClone)
        {
            if (itemsToClone == null)
            {
                throw new ArgumentNullException(nameof(itemsToClone));
            }

            var culture = Create(dto);
            culture.AddItems(itemsToClone);
            return culture;
        }
    }
}
