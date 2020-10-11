using Shared.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IAuthService
    {
        Task<IReadOnlyList<ClaimDto>> AuthenticateAsync(SecurityCredential credential);
    }
}