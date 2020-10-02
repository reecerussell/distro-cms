using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Controllers;
using Shared.Exceptions;
using Shared.Security;
using System;
using System.Threading.Tasks;
using Users.Infrastructure;

namespace API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _service;

        public AuthController(
            IAuthService service,
            ILogger<BaseController> logger) 
            : base(logger)
        {
            _service = service;
        }

        [HttpPost("password")]
        public async Task<IActionResult> VerifyPassword(PasswordGrantData grantData)
        {
            try
            {
                var claims = await _service.VerifyPasswordAsync(grantData);

                Logger.LogDebug("Password grant is valid, returning {0} claims.", claims.Count);

                return Ok(claims);
            }
            catch (AuthenticationFailedException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured while verifying a password grant.");

                return InternalServerError(e.Message);
            }
        }
    }
}
