using Microsoft.Extensions.Logging;
using Shared;
using Shared.Exceptions;
using System;
using System.Threading.Tasks;
using Users.Domain.Dtos;
using Users.Domain.Models;
using Users.Infrastructure.Repositories;

namespace Users.Infrastructure.Services
{
    internal class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly ILogger<RoleService> _logger;

        public RoleService(
            IRoleRepository repository,
            ILogger<RoleService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new role instance using the <paramref name="dto"/>.
        /// </summary>
        /// <param name="dto">The data required to create a new <see cref="Role"/>.</param>
        /// <returns>The id of the new <see cref="Role"/>.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="dto"/> is null.</exception>
        /// <exception cref="ValidationException">Throws if any of the data is invalid, or if the role's name has already been taken.</exception>
        public async Task<string> CreateAsync(CreateRoleDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            _logger.LogDebug("Creating a new role...");

            var role = Role.Create(dto);
            if (await _repository.ExistsWithNameAsync(role.Name))
            {
                _logger.LogDebug("Failed to create new role as the name '{0}' has already been taken.", role.Name);
                throw new ValidationException(ErrorMessages.RoleNameTaken);
            }

            _logger.LogDebug("Adding the new role to the repository...");

            _repository.Add(role);
            await _repository.SaveChangesAsync();

            _logger.LogDebug("Successfully created new role with id '{0}'", role.Id);

            return role.Id;
        }

        /// <summary>
        /// Updates an existing role using the data from the <paramref name="dto"/>.
        /// </summary>
        /// <param name="dto">The data required to update the <see cref="Role"/>.</param>
        /// <returns>Nothing :)</returns>
        /// <exception cref="ArgumentNullException">Throws if the <paramref name="dto"/> is null.</exception>
        /// <exception cref="ValidationException">Throws if any of the data is invalid, or if the role's name has already been taken.</exception>
        /// <exception cref="NotFoundException">Throws if the role cannot be found.</exception>
        public async Task UpdateAsync(UpdateRoleDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            _logger.LogDebug("Updating role '{0}'...", dto.Id);

            var role = await _repository.FindByIdAsync(dto.Id);
            if (role == null)
            {
                _logger.LogDebug("Could not find role with id '{0}'", dto.Id);

                throw new NotFoundException(ErrorMessages.RoleNotFound);
            }

            role.Update(dto);
            if (await _repository.ExistsWithNameAsync(role.Name, role.Id))
            {
                _logger.LogDebug("Failed to update role as the name '{0}' has already been taken.", role.Name);
                throw new ValidationException(ErrorMessages.RoleNameTaken);
            }

            _logger.LogDebug("Saving the role in the repository...");

            await _repository.SaveChangesAsync();

            _logger.LogDebug("Successfully saved the role with id '{0}'", role.Id);
        }

        /// <summary>
        /// Deletes an existing role with the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the role to be deleted.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws if the <paramref name="id"/> is null.</exception>
        /// <exception cref="NotFoundException">Throws if the role cannot be found.</exception>
        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(id);
            }

            _logger.LogDebug("Deleting role '{0}'...", id);

            var role = await _repository.FindByIdAsync(id);
            if (role == null)
            {
                _logger.LogDebug("Could not find role with id '{0}'", id);

                throw new NotFoundException(ErrorMessages.RoleNotFound);
            }

            _logger.LogDebug("Removing role from the repository...");

            _repository.Remove(role);
            await _repository.SaveChangesAsync();

            _logger.LogDebug("Successfully deleted the role with id '{0}'", id);
        }
    }
}
