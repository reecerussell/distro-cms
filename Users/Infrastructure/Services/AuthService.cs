using Shared;
using Shared.Exceptions;
using Shared.Passwords;
using Shared.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Users.Domain.Models;
using Users.Infrastructure.Repositories;

namespace Users.Infrastructure.Services
{
    internal class AuthService : IAuthService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository repository,
            IPasswordHasher passwordHasher,
            ILogger<AuthService> logger)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<IReadOnlyList<Claim>> VerifyPasswordAsync(PasswordGrantData grantData)
        {
            _logger.LogDebug("Verifying password grant...");

            EnsureAudienceIsValid(grantData.Audience);

            var user = await _repository.FindByEmailWithRolesAsync(grantData.Email);
            if (user == null)
            {
                _logger.LogDebug("No user could be found with the email '{0}'.", grantData.Email);

                throw new AuthenticationFailedException(ErrorMessages.AuthInvalidCredentials);
            }

            if (!user.VerifyPassword(grantData.Password, _passwordHasher))
            {
                _logger.LogDebug("The given password is not valid for the user with email '{0}'", grantData.Email);

                throw new AuthenticationFailedException(ErrorMessages.AuthInvalidCredentials);
            }

            return BuildClaims(user);
        }

        private static IReadOnlyList<Claim> BuildClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("user_id", user.Id), 
                new Claim("name", user.Firstname)
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim("role", role.RoleId));
            }

            return claims;
        }

        private void EnsureAudienceIsValid(string audience)
        {
            _logger.LogDebug("Ensuring the audience '{0}' is valid...", audience);

            if (string.IsNullOrEmpty(audience))
            {
                throw new ArgumentNullException(nameof(audience));
            }

            if (audience == "cms")
            {
                return;
            }

            _logger.LogDebug("The audience '{0}' is not a valid audience.", audience);

            throw new AuthenticationFailedException(ErrorMessages.AuthInvalidAudience);
        }
    }
}
