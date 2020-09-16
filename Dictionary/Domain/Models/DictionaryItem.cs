using Dictionary.Domain.Dtos;
using Shared;
using Shared.Entity;
using Shared.Exceptions;
using System;
using System.Globalization;

namespace Dictionary.Domain.Models
{
    public class DictionaryItem : Aggregate
    {
        public string CultureName { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }

        private DictionaryItem()
        {
        }

        private DictionaryItem(CultureInfo culture, string key)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            CultureName = culture.Name;
            SetKey(key);
        }

        internal void SetKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ValidationException(ErrorMessages.DictionaryKeyRequired);
            }

            if (key.Length > 45)
            {
                throw new ValidationException(ErrorMessages.DictionaryKeyTooLong);
            }

            Key = key;
        }

        internal void UpdateValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ValidationException(ErrorMessages.DictionaryValueRequired);
            }

            if (value.Length > 255)
            {
                throw new ValidationException(ErrorMessages.DictionaryValueTooLong);
            }

            Value = value;
        }

        public void Update(UpdateDictionaryItem dto)
        {
            UpdateValue(dto.Value);
        }

        public static DictionaryItem Create(CreateDictionaryItem dto, CultureInfo culture)
        {
            var item = new DictionaryItem(culture, dto.Key);
            item.UpdateValue(dto.Value);
            return item;
        }
    }
}
