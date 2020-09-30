using Dictionary.Domain.Dtos;
using Dictionary.Domain.Models;
using Dictionary.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Exceptions;
using System.Threading.Tasks;

namespace Dictionary.Infrastructure.Services
{
    internal class SupportedCultureService : ISupportedCultureService
    {
        private readonly ISupportedCultureRepository _repository;
        private readonly IDictionaryItemRepository _dictionaryItemRepository;
        private readonly ILogger<SupportedCultureService> _logger;

        public SupportedCultureService(
            ISupportedCultureRepository repository,
            IDictionaryItemRepository dictionaryItemRepository,
            ILogger<SupportedCultureService> logger)
        {
            _repository = repository;
            _dictionaryItemRepository = dictionaryItemRepository;
            _logger = logger;
        }

        public async Task<string> CreateAsync(CreateSupportedCultureDto dto)
        {
            SupportedCulture supportedCulture;
            if (string.IsNullOrEmpty(dto.CloneCultureId))
            {
                _logger.LogDebug("Creating a new supported culture.");

                supportedCulture = SupportedCulture.Create(dto);
            }
            else
            {
                _logger.LogDebug("Creating a new supported culture, based on culture '{0}'", dto.CloneCultureId);

                var itemsToClone = await _dictionaryItemRepository.GetItemsToCloneByCultureAsync(dto.CloneCultureId);
                supportedCulture = SupportedCulture.Create(dto, itemsToClone);
            }
            
            if (await _repository.ExistsWithNameAsync(supportedCulture.Name))
            {
                _logger.LogDebug("Failed to create a new supported culture as the name '{0}' already exists.");

                throw new ValidationException(ErrorMessages.SupportedCultureAlreadyExists);
            }

            _repository.Add(supportedCulture);
            await _repository.SaveChangesAsync();

            _logger.LogDebug("Successfully created a new supported culture '{0}'", supportedCulture.Id);

            return supportedCulture.Id;
        }

        public async Task SetAsDefaultAsync(string id)
        {
            _logger.LogDebug("Setting supported culture '{0}' as default...", id);

            _logger.LogDebug("Setting default cultures to not default...");
            var defaultCultures = await _repository.GetDefaultCulturesAsync();
            foreach (var culture in defaultCultures)
            {
                culture.SetAsNotDefault();
            }

            var supportedCulture = await _repository.FindByIdAsync(id);
            if (supportedCulture == null)
            {
                _logger.LogDebug("Failed to set the supported culture as default, as it does not exist.");

                throw new NotFoundException(ErrorMessages.SupportedCultureNotFound);
            }

            supportedCulture.SetAsDefault();
            
            await _repository.SaveChangesAsync();

            _logger.LogDebug("Successfully set supported culture '{0}' as default.", id);
        }

        public async Task UpdateAsync(UpdateSupportedCultureDto dto)
        {
            _logger.LogDebug("Updating supported culture '{0}'", dto.Id);

            var supportedCulture = await _repository.FindByIdAsync(dto.Id);
            if (supportedCulture == null)
            {
                _logger.LogDebug("Failed to update supported culture as it could not be found.");

                throw new NotFoundException(ErrorMessages.SupportedCultureNotFound);
            }

            supportedCulture.Update(dto);
            await _repository.SaveChangesAsync();

            _logger.LogDebug("Successfully updated supported culture '{0}'", dto.Id);
        }

        public async Task DeleteAsync(string id)
        {
            _logger.LogDebug("Deleting supported culture '{0}'", id);

            var supportedCulture = await _repository.FindByIdAsync(id);
            if (supportedCulture == null)
            {
                _logger.LogDebug("Failed to delete supported culture as it could not be found.");

                throw new NotFoundException(ErrorMessages.SupportedCultureNotFound);
            }

            _repository.Remove(supportedCulture);
            await _repository.SaveChangesAsync();

            _logger.LogDebug("Successfully deleted supported culture '{0}'", id);
        }
    }
}
