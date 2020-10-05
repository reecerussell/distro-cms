using Shared.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface ITokenService
    {
        Task<Token> GenerateAsync(IReadOnlyList<ClaimDto> claims);
    }
}