using System;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Controllers;
using Shared.Exceptions;
using Shared.Localization;
using Shared.Security;

namespace API.Controllers
{
    public class OAuthController : BaseController
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;
        private readonly ILocalizer _localizer;

        public OAuthController(
            ITokenService tokenService,
            IAuthService authService,
            ILocalizer localizer,
            ILogger<BaseController> logger) 
            : base(logger)
        {
            _tokenService = tokenService;
            _authService = authService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Token(SecurityCredential credential)
        {
            try
            {
                var claims = await _authService.AuthenticateAsync(credential);
                var token = _tokenService.GenerateAsync(claims);

                return Ok(token);
            }
            catch (AuthenticationFailedException e)
            {
                Logger.LogDebug("Authentication failed: {0}", e.Message);

                return BadRequest(await _localizer.GetErrorAsync(e.Message));
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured while attempting to authenticate user.");

                return InternalServerError(e.Message);
            }
        }
    }
}
