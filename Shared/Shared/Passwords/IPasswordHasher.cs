namespace Shared.Passwords
{
    public interface IPasswordHasher
    {
        string Hash(string pwd);
        bool Verify(string pwd, string base64Hash);
    }
}