namespace Shared.Passwords
{
    public interface IPasswordGenerator
    {
        string Generate(int length);
    }
}
