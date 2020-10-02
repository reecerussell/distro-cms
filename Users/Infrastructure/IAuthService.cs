using Shared.Security;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Users.Infrastructure
{
    public interface IAuthService
    {
        Task<IReadOnlyList<Claim>> VerifyPasswordAsync(PasswordGrantData grantData);
    }
}