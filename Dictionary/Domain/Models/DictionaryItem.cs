﻿using Dictionary.Domain.Dtos;
using Shared;
using Shared.Entity;
using Shared.Exceptions;
using System;

namespace Dictionary.Domain.Models
{
    public class DictionaryItem : Aggregate
    {
        public string CultureId { get; private set; }
        public string Key { get; private set; }
        public string DisplayName { get; private set; }
        public string Value { get; private set; }

        public SupportedCulture Culture { get; private set; }

        private DictionaryItem()
        {
        }

        private DictionaryItem(SupportedCulture culture, string key)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            Culture = culture;
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

        internal void UpdateDisplayName(string displayName)
        {
            if (string.IsNullOrEmpty(displayName))
            {
                throw new ValidationException(ErrorMessages.DictionaryDisplayNameRequired);
            }

            if (displayName.Length > 255)
            {
                throw new ValidationException(ErrorMessages.DictionaryDisplayNameTooLong);
            }

            DisplayName = displayName;
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
            UpdateDisplayName(dto.DisplayName);
        }

        public static DictionaryItem Create(CreateDictionaryItem dto, SupportedCulture culture)
        {
            var item = new DictionaryItem(culture, dto.Key);
            item.UpdateValue(dto.Value);
            item.UpdateDisplayName(dto.DisplayName);
            return item;
        }
    }
}
