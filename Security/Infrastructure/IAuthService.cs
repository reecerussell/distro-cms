using Shared.Security;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IAuthService
    {
        Task<IReadOnlyList<Claim>> AuthenticateAsync(SecurityCredential credential);
    }
}