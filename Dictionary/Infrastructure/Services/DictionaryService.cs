using Dictionary.Domain.Dtos;
using Dictionary.Domain.Models;
using Dictionary.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Exceptions;
using System.Globalization;
using System.Threading.Tasks;

namespace Dictionary.Infrastructure.Services
{
    internal class DictionaryService : IDictionaryService
    {
        private readonly IDictionaryItemRepository _repository;
        private readonly ISupportedCultureRepository _cultureRepository;
        private readonly ILogger<DictionaryService> _logger;

        public DictionaryService(
            IDictionaryItemRepository repository,
            ISupportedCultureRepository cultureRepository,
            ILogger<DictionaryService> logger)
        {
            _repository = repository;
            _cultureRepository = cultureRepository;
            _logger = logger;
        }

        public async Task<string> CreateAsync(CreateDictionaryItem dto, CultureInfo culture)
        {
            _logger.LogDebug("Creating a new dictionary item...");

            var supportedCulture = await _cultureRepository.FindByNameAsync(culture.Name);
            if (supportedCulture == null)
            {
                throw new ValidationException(ErrorMessages.DictionaryUnsupportedCulture);
            }

            var item = DictionaryItem.Create(dto, supportedCulture);
            if (await _repository.ExistsAsync(item.Key, supportedCulture))
            {
                _logger.LogDebug("Failed to create a dictionary item as one already exists with culture '{0}' and key '{1}'",
                    culture.Name, item.Key);

                throw new ValidationException(ErrorMessages.DictionaryItemAlreadyExists);
            }

            _logger.LogDebug("Saving dictionary item in repository...");

            _repository.Add(item);
            await _repository.SaveChangesAsync();

            _logger.LogDebug("Successfully created dictionary item.");

            return item.Id;
        }

        public async Task UpdateAsync(UpdateDictionaryItem dto)
        {
            _logger.LogDebug("Updating dictionary item...");

            var item = await _repository.FindByIdAsync(dto.Id);
            if (item == null)
            {
                _logger.LogDebug("Could not find diction item '{0}'", dto.Id);

                throw new NotFoundException(ErrorMessages.DictionaryItemNotFound);
            }

            item.Update(dto);

            _logger.LogDebug("Saving updated item to the repository...");

            await _repository.SaveChangesAsync();

            _logger.LogDebug("Successfully updated dictionary item.");
        }

        public async Task DeleteAsync(string id)
        {
            _logger.LogDebug("Deleting dictionary item...");

            var item = await _repository.FindByIdAsync(id);
            if (item == null)
            {
                _logger.LogDebug("Could not find diction item '{0}'", id);

                throw new NotFoundException(ErrorMessages.DictionaryItemNotFound);
            }

            _logger.LogDebug("Removing item from the repository...");

            _repository.Remove(item);
            await _repository.SaveChangesAsync();

            _logger.LogDebug("Successfully deleted dictionary item.");
        }
    }
}
