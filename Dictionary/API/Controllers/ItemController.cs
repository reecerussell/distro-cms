using Dictionary.Domain.Dtos;
using Dictionary.Infrastructure;
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
    [Route("api/items")]
    public class ItemController : BaseController
    {
        private readonly IDictionaryService _service;
        private readonly IDictionaryItemProvider _provider;

        public ItemController(
            IDictionaryService service,
            IDictionaryItemProvider provider,
            ILogger<BaseController> logger) 
            : base(logger)
        {
            _service = service;
            _provider = provider;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var items = await _provider.GetListAsync(CultureInfo.CurrentCulture);

                return Ok(items);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured fetching dictionary items for culture: {0}", CultureInfo.CurrentCulture.Name);

                return InternalServerError(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var item = await _provider.GetAsync(id);

                return Ok(item);
            }
            catch (NotFoundException e)
            {
                Logger.LogDebug("Could not find dictionary item '{0}'", id);

                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured fetching dictionary item '{0}'", id);

                return InternalServerError(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDictionaryItem dto)
        {
            try
            {
                var itemId = await _service.CreateAsync(dto, CultureInfo.CurrentCulture);

                return RedirectToAction(nameof(Get), new {id = itemId});
            }
            catch (ValidationException e)
            {
                Logger.LogDebug("A validation error occured while creating a dictionary item: {0}", e.Message);

                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An unknown error occured while creating an item: {0}", e.Message);

                return InternalServerError(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateDictionaryItem dto)
        {
            try
            {
                await _service.UpdateAsync(dto);

                return await Get(dto.Id);
            }
            catch (NotFoundException e)
            {
                Logger.LogDebug("Could not find dictionary item '{0}'", dto.Id);

                return NotFound(e.Message);
            }
            catch (ValidationException e)
            {
                Logger.LogDebug("A validation error occured while updating a dictionary item: {0}", e.Message);

                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An unknown error occured while updating an item: {0}", e.Message);

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
                Logger.LogDebug("Could not find dictionary item '{0}'", id);

                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An unknown error occured while deleting an item: {0}", e.Message);

                return InternalServerError(e.Message);
            }
        }
    }
}
