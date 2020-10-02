namespace Shared.Security
{
    /// <summary>
    /// An object containing credential properties for OAuth.
    /// </summary>
    public class SecurityCredential
    {
        public string GrantType { get; set; }
        public string Audience { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // TODO: support refresh token and auth code grant types.
        //public string RefreshToken { get; set; }
        //public string AuthCode { get; set; }
    }
}
