namespace Shared.Security
{
    public class PasswordGrantData
    {
        public PasswordGrantData()
        {
        }

        public PasswordGrantData(string email, string password, string audience)
        {
            Email = email;
            Password = password;
            Audience = audience;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Audience { get; set; }
    }
}
