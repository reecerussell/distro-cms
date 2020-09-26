using Shared;
using Shared.Entity;
using Shared.Exceptions;
using System.Globalization;

namespace Dictionary.Domain.Models
{
    public class SupportedCulture : Aggregate
    {
        public string Name { get; protected set; }
        public string DisplayName { get; private set; }
        public bool IsDefault { get; private set; }

        private SupportedCulture()
        {
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
        }
    }
}
