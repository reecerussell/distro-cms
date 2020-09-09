namespace Shared.OAuth
{
    public class AccessToken
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public double Expires { get; set; }
    }
}
