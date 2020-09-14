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
    [Route("api/roles")]
    public class RolesController : BaseController
    {
        private readonly IRoleService _service;

        public RolesController(
            IRoleService service,
            ILogger<BaseController> logger) 
            : base(logger)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleDto dto)
        {
            try
            {
                var roleId = await _service.CreateAsync(dto);

                return Ok(roleId);
            }
            catch (ValidationException e)
            {
                Logger.LogDebug("A validation error occured while creating a role: {0}", e.Message);

                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured while creating a role.");

                return InternalServerError(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateRoleDto dto)
        {
            try
            {
                await _service.UpdateAsync(dto);

                return Ok();
            }
            catch (ValidationException e)
            {
                Logger.LogDebug("A validation error occured while updating role '{0}': {1}", dto.Id, e.Message);

                return BadRequest(e.Message);
            }
            catch (NotFoundException e)
            {
                Logger.LogDebug("Failed to update role '{0}' as it could not be found.", dto.Id);

                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured while updating role '{0}'", dto.Id);

                return InternalServerError(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _service.DeleteAsync(id);

                return Ok();
            }
            catch (NotFoundException e)
            {
                Logger.LogDebug("Failed to delete role '{0}' as it could not be found: {1}", id, e.Message);

                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured while deleting role '{0}': {1}", id, e.Message);

                return InternalServerError(e.Message);
            }
        }
    }
}
