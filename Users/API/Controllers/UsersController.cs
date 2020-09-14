using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Controllers;
using Shared.Exceptions;
using System;
using System.Threading.Tasks;
using Users.Domain.Dtos;
using Users.Infrastructure;

namespace API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : BaseController
    {
        private readonly IUserService _service;
        private readonly IUserProvider _provider;

        public UsersController(
            IUserService service,
            IUserProvider provider,
            ILogger<BaseController> logger) 
            : base(logger)
        {
            _service = service;
            _provider = provider;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Logger.LogDebug("Requesting user by id '{0}'", id);

            try
            {
                var user = await _provider.GetAsync(id);

                Logger.LogDebug("User with id '{0}' found.", id);

                return Ok(user);
            }
            catch (NotFoundException e)
            {
                Logger.LogDebug("No user could be found with the id '{0}'", id);

                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to get user by id '{0}'", id);

                return InternalServerError(e.Message);
            }
        }

        // TODO: list
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            try
            {
                var userId = await _service.CreateAsync(dto);

                Logger.LogDebug("Successfully created user with id '{0}'.", userId);

                return Ok(userId);
            }
            catch (ValidationException e)
            {
                Logger.LogDebug("A validation occured while creating a user: {0}", e.Message);

                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured while creating a user.");

                return InternalServerError(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserDto dto)
        {
            try
            {
                await _service.UpdateAsync(dto);

                Logger.LogDebug("Successfully updated user with id '{0}'.", dto.Id);

                return Ok();
            }
            catch (NotFoundException e)
            {
                Logger.LogDebug("Could not update user as no user could be found with id '{0}'.", dto.Id);

                return NotFound(e.Message);
            }
            catch (ValidationException e)
            {
                Logger.LogDebug("A validation occured while updating the user: {0}", e.Message);

                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured while updating the user.");

                return InternalServerError(e.Message);
            }
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            try
            {
                await _service.ChangePasswordAsync(dto);

                return Ok();
            }
            catch (NotFoundException e)
            {
                Logger.LogDebug("Could not change users '{0}' password as they could not be found.", dto.Id);

                return NotFound(e.Message);
            }
            catch (ValidationException e)
            {
                Logger.LogDebug("A validation occured while changing user's '{0}' password.", e.Message);

                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured while changing the user's password");

                return InternalServerError(e.Message);
            }
        }
    }
}
