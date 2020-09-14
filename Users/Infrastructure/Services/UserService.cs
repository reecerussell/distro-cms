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
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPasswordValidator _passwordValidator;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository repository,
            IPasswordHasher passwordHasher,
            IPasswordValidator passwordValidator,
            ILogger<UserService> logger)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new user, validating the data given, then saves it to the data source.
        /// </summary>
        /// <param name="dto">The data required to create a user.</param>
        /// <returns>The newly created user's id.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="dto"/> is null.</exception>
        /// <exception cref="ValidationException">Throws if the data is invalid.</exception>
        public async Task<string> CreateAsync(CreateUserDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            _logger.LogDebug("Creating user with email: {0}", dto.Email);

            var user = User.Create(dto, _passwordHasher, _passwordValidator);
            if (await _repository.ExistsWithEmailAsync(user.Email))
            {
                _logger.LogDebug("The email address '{0}' has already been taken.", user.Email);

                throw new ValidationException(ErrorMessages.UserEmailTaken);
            }

            _logger.LogDebug("Adding to repository, then saving...");
            _repository.Add(user);
            await _repository.SaveChangesAsync();

            _logger.LogDebug("Created user, with id: {0}", user.Id);

            return user.Id;
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

            if (await _repository.ExistsWithEmailAsync(user.Email))
            {
                _logger.LogDebug("The email address '{0}' has already been taken.", user.Email);

                throw new ValidationException(ErrorMessages.UserEmailTaken);
            }

            await _repository.SaveChangesAsync();

            _logger.LogDebug("Successfully updated user with id '{0}'.", user.Id);
        }

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
    }
}
