using Shared.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Users.Infrastructure
{
    public interface IAuthService
    {
        Task<IReadOnlyList<ClaimDto>> VerifyPasswordAsync(PasswordGrantData grantData);
    }
}