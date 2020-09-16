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
        private readonly IRoleProvider _provider;

        public RolesController(
            IRoleService service,
            IRoleProvider provider,
            ILogger<BaseController> logger) 
            : base(logger)
        {
            _service = service;
            _provider = provider;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(string term = null)
        {
            try
            {
                var roles = await _provider.GetListAsync(term);

                return Ok(roles);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured while getting a list of roles.");

                return InternalServerError(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var role = await _provider.GetAsync(id);

                return Ok(role);
            }
            catch (NotFoundException e)
            {
                Logger.LogDebug("Failed to get role with id '{0}': {1}", id, e.Message);

                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured while getting a role by id '{0}'", id);

                return InternalServerError(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleDto dto)
        {
            try
            {
                var roleId = await _service.CreateAsync(dto);

                return RedirectToAction(nameof(Get), new {id = roleId});
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

                return await Get(dto.Id);
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
