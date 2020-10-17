namespace Shared.Passwords
{
    public class PasswordOptions
    {
        public PasswordHasherOptions Hasher { get; set; }
        public PasswordValidationOptions Validation { get; set; }
        public PasswordGeneratorOptions Generator { get; set; }

        public PasswordOptions()
        {
            Hasher = new PasswordHasherOptions();
            Validation = new PasswordValidationOptions();
            Generator = new PasswordGeneratorOptions();
        }
    }
}
