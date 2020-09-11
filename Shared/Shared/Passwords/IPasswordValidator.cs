namespace Shared.Passwords
{
    public interface IPasswordValidator
    {
        /// <summary>
        /// Validates the given password.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <exception cref="Exceptions.ValidationException">Throws if the password is not valid.</exception>
        void Validate(string password);
    }
}
