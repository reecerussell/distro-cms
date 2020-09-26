using System.Threading.Tasks;

namespace Shared.Localization
{
    public interface ILocalizer
    {
        /// <summary>
        /// Attempts to retrieve a localized string for the current culture,
        /// using the key given
        /// </summary>
        /// <param name="key">The key of the localized string.</param>
        /// <returns>A localized string.</returns>
        Task<string> GetStringAsync(string key);

        Task<string> GetErrorAsync(string key);
    }
}
