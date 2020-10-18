using Microsoft.Extensions.Logging;
using Shared;
using Shared.Exceptions;
using Shared.Passwords;
using System;
using System.Threading.Tasks;
using Users.Domain.Dtos;
using Users.Domain.Models;
using Users.Infrastructure.Repositories;

namespace Users.Infrastructure.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPasswordValidator _passwordValidator;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository repository,
            IRoleRepository roleRepository,
            IPasswordHasher passwordHasher,
            IPasswordValidator passwordValidator,
            IPasswordGenerator passwordGenerator,
            ILogger<UserService> logger)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _passwordGenerator = passwordGenerator;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new user, validating the data given, then saves it to the data source.
        /// </summary>
        /// <param name="dto">The data required to create a user.</param>
        /// <returns>The newly created user's id.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="dto"/> is null.</exception>
        /// <exception cref="ValidationException">Throws if the data is invalid.</exception>
        /// <returns>The newly created user's id and randomly generated password.</returns>
        public async Task<(string id, string password)> CreateAsync(CreateUserDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            _logger.LogDebug("Creating user with email: {0}", dto.Email);

            var (user, password) = User.Create(dto, _passwordHasher, _passwordGenerator);
            if (await _repository.ExistsWithEmailAsync(user.Email))
            {
                _logger.LogDebug("The email address '{0}' has already been taken.", user.Email);

                throw new ValidationException(ErrorMessages.UserEmailTaken);
            }

            _logger.LogDebug("Adding to repository, then saving...");
            _repository.Add(user);

            await _repository.SaveChangesAsync();

            _logger.LogDebug("Created user, with id: {0}", user.Id);

            return (user.Id, password);
        }

        /// <summary>
        /// Updates an existing user's core data, then saves it to the data source.
        /// </summary>
        /// <param name="dto">The data required to update a user.</param>
        /// <returns>Nothing :)</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="dto"/> is null.</exception>
        /// <exception cref="ValidationException">Throws if the data is invalid.</exception>
        /// <exception cref="NotFoundException">Throws if the user does not exist.</exception>
        public async Task UpdateAsync(UpdateUserDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            _logger.LogDebug("Updating user '{0}'...", dto.Id);

            var user = await _repository.FindByIdAsync(dto.Id);
            if (user == null)
            {
                _logger.LogDebug("User with id '{0}' could not be found.", dto.Id);

                throw new NotFoundException(ErrorMessages.UserNotFound);
            }

            user.Update(dto);

            if (await _repository.ExistsWithEmailAsync(user.Email, user.Id))
            {
                _logger.LogDebug("The email address '{0}' has already been taken.", user.Email);

                throw new ValidationException(ErrorMessages.UserEmailTaken);
            }

            await _repository.SaveChangesAsync();

            _logger.LogDebug("Successfully updated user with id '{0}'.", user.Id);
        }

        /// <summary>
        /// Changes the password of a user with the given id. Ensured the current password is valid.
        /// </summary>
        /// <param name="dto">Contains a user's id, new password and current password.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="dto"/> is null.</exception>
        /// <exception cref="NotFoundException">Throws if the user with the given id cannot be found.</exception>
        /// <exception cref="ValidationException">Throws if the user's current password cannot be verified or if the new password is not valid.</exception>
        public async Task ChangePasswordAsync(ChangePasswordDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            _logger.LogDebug("Changing password for user '{0}'", dto.Id);

            var user = await _repository.FindByIdAsync(dto.Id);
            if (user == null)
            {
                _logger.LogDebug("User with id '{0}' could not be found.", dto.Id);

                throw new NotFoundException(ErrorMessages.UserNotFound);
            }

            user.UpdatePassword(dto, _passwordHasher, _passwordValidator);

            await _repository.SaveChangesAsync();

            _logger.LogDebug("Successfully change user's password.");
        }

        /// <summary>
        /// Assigns the given user to a role.
        /// </summary>
        /// <param name="dto">Contains the user's id and the target role.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="dto"/> is null.</exception>
        /// <exception cref="NotFoundException">Throws if the user or role cannot be found.</exception>
        /// <exception cref="ValidationException">Throws if the user cannot be assigned to the role.</exception>
        public async Task AddToRoleAsync(UserRoleDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            _logger.LogDebug("Adding user '{0}' to role '{1}'", dto.UserId, dto.RoleId);

            var user = await _repository.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                _logger.LogDebug("Could not find user with id '{0}'", dto.UserId);

                throw new NotFoundException(ErrorMessages.UserNotFound);
            }

            var role = await _roleRepository.FindByIdAsNoTrackingAsync(dto.RoleId);
            if (role == null)
            {
                _logger.LogDebug("Could not find role with id '{0}'", dto.RoleId);

                throw new NotFoundException(ErrorMessages.RoleNotFound);
            }

            user.AddRole(role.Id);

            _logger.LogDebug("Saving the changes to the repository...");
            
            await _repository.SaveChangesAsync();

            _logger.LogDebug("Successfully added the user '{0}' to the role '{1}'", user.Id, role.Id);
        }

        /// <summary>
        /// Removed the user from the given role.
        /// </summary>
        /// <param name="dto">Contains the user's id and the target role.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="dto"/> is null.</exception>
        /// <exception cref="NotFoundException">Throws if the user or role cannot be found.</exception>
        /// <exception cref="ValidationException">Throws if the user cannot be removed from the role.</exception>
        public async Task RemoveFromRoleAsync(UserRoleDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            _logger.LogDebug("Removing user '{0}' from role '{1}'", dto.UserId, dto.RoleId);

            var user = await _repository.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                _logger.LogDebug("Could not find user with id '{0}'", dto.UserId);

                throw new NotFoundException(ErrorMessages.UserNotFound);
            }

            var role = await _roleRepository.FindByIdAsNoTrackingAsync(dto.RoleId);
            if (role == null)
            {
                _logger.LogDebug("Could not find role with id '{0}'", dto.RoleId);

                throw new NotFoundException(ErrorMessages.RoleNotFound);
            }

            user.RemoveRole(role.Id);

            _logger.LogDebug("Saving the changes to the repository...");

            await _repository.SaveChangesAsync();

            _logger.LogDebug("Successfully removed the user '{0}' from the role '{1}'", user.Id, role.Id);
        }

        /// <summary>
        /// Deletes the user with the <paramref name="id"/>, along with any related entities, such as roles etc.
        /// </summary>
        /// <param name="id">The id of the user to be deleted.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="id"/> is null or empty.</exception>
        /// <exception cref="NotFoundException">Throws is no user could be found with this given <paramref name="id"/>.</exception>
        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            _logger.LogDebug("Deleting user '{0}'...", id);

            var user = await _repository.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogDebug("Could not find user with id '{0}'", id);

                throw new NotFoundException(ErrorMessages.UserNotFound);
            }

            _repository.Remove(user);
            await _repository.SaveChangesAsync();

            _logger.LogDebug("Successfully deleted user '{0}'.", id);
        }
    }
}
