namespace Shared.Passwords
{
    public class PasswordHasherOptions
    {
        public int IterationCount { get; set; }
        public int KeySize { get; set; }
        public int SaltSize { get; set; }
    }
}
