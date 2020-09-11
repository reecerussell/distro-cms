using Shared;
using Shared.Entity;
using Shared.Exceptions;
using Shared.Passwords;
using System.Text.RegularExpressions;
using Users.Domain.Dtos;

namespace Users.Domain.Models
{
    public class User : Aggregate
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        internal void UpdateFirstname(string firstname)
        {
            if (string.IsNullOrEmpty(firstname))
            {
                throw new ValidationException(ErrorMessages.UserFirstnameRequired);
            }

            if (firstname.Length > 255)
            {
                throw new ValidationException(ErrorMessages.UserFirstnameTooLong);
            }

            Firstname = firstname;
        }

        internal void UpdateLastname(string lastname)
        {
            if (string.IsNullOrEmpty(lastname))
            {
                throw new ValidationException(ErrorMessages.UserLastnameRequired);
            }

            if (lastname.Length > 255)
            {
                throw new ValidationException(ErrorMessages.UserLastnameTooLong);
            }

            Lastname = lastname;
        }

        internal void UpdateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ValidationException(ErrorMessages.UserEmailRequired);
            }

            if (email.Length > 255)
            {
                throw new ValidationException(ErrorMessages.UserEmailTooLong);
            }

            var match = Regex.IsMatch(email, "[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,6}");
            if (!match)
            {
                throw new ValidationException(ErrorMessages.UserEmailInvalid);
            }

            Email = email.ToLowerInvariant();
        }

        /// <summary>
        /// Updates the user's password as long as the current password is correct. The user's password must
        /// be valid, which is determined by the <paramref name="validator"/>.
        /// </summary>
        /// <param name="dto">A DTO containing the user's new and current passwords.</param>
        /// <param name="hasher">The hasher used to hash the password.</param>
        /// <param name="validator">The password validator.</param>
        /// <exception cref="ValidationException">Throws a <see cref="ValidationException"/> is the current password is not verified or if the new password is not valid.</exception>
        internal void UpdatePassword(ChangePasswordDto dto, IPasswordHasher hasher, IPasswordValidator validator)
        {
            if (string.IsNullOrEmpty(dto.NewPassword))
            {
                throw new ValidationException(ErrorMessages.UserPasswordRequired);
            }

            if (string.IsNullOrEmpty(dto.CurrentPassword))
            {
                throw new ValidationException(ErrorMessages.UserConfirmPasswordRequired);
            }

            if (!hasher.Verify(dto.CurrentPassword, PasswordHash))
            {
                throw new ValidationException(ErrorMessages.UserPasswordInvalid);
            }

            SetPassword(dto.NewPassword, hasher, validator);
        }

        /// <summary>
        /// Sets the user's password, ensuring it is also valid.
        /// </summary>
        /// <param name="password">The password to be set.</param>
        /// <param name="hasher">The hasher used to hash the password.</param>
        /// <param name="validator">The password validator.</param>
        /// <exception cref="ValidationException">Throws a ValidationException is the password is invalid.</exception>
        private void SetPassword(string password, IPasswordHasher hasher, IPasswordValidator validator)
        {
            validator.Validate(password);
            PasswordHash = hasher.Hash(password);
        }

        /// <summary>
        /// Creates a new instance of <see cref="User"/> ensuring all of the data is valid.
        /// </summary>
        /// <param name="dto">A DTO containing the data needed to create a user.</param>
        /// <param name="hasher">The hasher used to hash the password.</param>
        /// <param name="validator">The password validator.</param>
        /// <returns>A new instance of <see cref="User"/>.</returns>
        /// <exception cref="ValidationException">Throws if any of the given data is invalid.</exception>
        public static User Create(CreateUserDto dto, IPasswordHasher hasher, IPasswordValidator validator)
        {
            var user = new User();
            user.UpdateFirstname(dto.Firstname);
            user.UpdateLastname(dto.Lastname);
            user.UpdateEmail(dto.Email);
            user.SetPassword(dto.Password, hasher, validator);

            return user;
        }
    }
}
