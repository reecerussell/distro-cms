using Domain.Dtos;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Controllers;
using Shared.Exceptions;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/pages")]
    public class PagesController : BaseController
    {
        private readonly IPageService _service;
        private readonly IPageProvider _provider;
        
        public PagesController(
            IPageService service,
            IPageProvider provider,
            ILogger<PagesController> logger) 
            : base(logger)
        {
            _service = service;
            _provider = provider;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var culture = CultureInfo.CurrentCulture;
            Logger.LogDebug("Retrieving page '{0}' for culture: {1}", id, culture.Name);

            try
            {
                var page = await _provider.GetPageAsync(id, culture);

                return Ok(page);
            }
            catch (NotFoundException e)
            {
                Logger.LogDebug("Could not find page with id '{0}', for the culture: {1}", id, culture.Name);

                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An exception occured while retrieving page '{0}', for the culture: {1}", id, culture.Name);

                return InternalServerError(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePageDto dto)
        {
            try
            {
                var id = await _service.CreateAsync(dto, CultureInfo.CurrentCulture);

                return Ok(id);
            }
            catch (ValidationException e)
            {
                Logger.LogDebug("A validation exception occured while creating a page: {0}", e.Message);

                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An exception occured while creating a page");

                return InternalServerError(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdatePageDto dto)
        {
            var culture = CultureInfo.CurrentCulture;
            Logger.LogDebug("Updating page '{0}' for culture: {1}", dto.Id, culture.Name);

            try
            {
                await _service.UpdateAsync(dto, culture);

                return Ok(null);
            }
            catch (NotFoundException e)
            {
                Logger.LogDebug("Could not find page with id '{0}'", dto.Id);

                return NotFound(e.Message);
            }
            catch (ValidationException e)
            {
                Logger.LogDebug("A validation exception occured while updating page with id '{0}': {1}", dto.Id, e.Message);

                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An exception occured while updating page '{0}'", dto.Id);

                return InternalServerError(e.Message);
            }
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> Activate(string id)
        {
            try
            {
                await _service.ActivateAsync(id);

                return Ok(null);
            }
            catch (NotFoundException e)
            {
                Logger.LogDebug("Could not find page with id '{0}'", id);

                return NotFound(e.Message);
            }
            catch (ValidationException e)
            {
                Logger.LogDebug("A validation exception occured while activating page with id '{0}': {1}", id, e.Message);

                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An exception occured while activating page '{0}'", id);

                return InternalServerError(e.Message);
            }
        }

        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(string id)
        {
            try
            {
                await _service.DeactivateAsync(id);

                return Ok(null);
            }
            catch (NotFoundException e)
            {
                Logger.LogDebug("Could not find page with id '{0}'", id);

                return NotFound(e.Message);
            }
            catch (ValidationException e)
            {
                Logger.LogDebug("A validation exception occured while deactivating page with id '{0}': {1}", id, e.Message);

                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An exception occured while deactivating page '{0}'", id);

                return InternalServerError(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _service.DeleteAsync(id);

                return Ok(null);
            }
            catch (NotFoundException e)
            {
                Logger.LogDebug("Could not find page with id '{0}'", id);

                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An exception occured while deleting page with id '{0}'", id);

                return InternalServerError(e.Message);
            }
        }
    }
}
