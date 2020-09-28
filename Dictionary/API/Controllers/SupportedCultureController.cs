using Dictionary.Domain.Dtos;
using Dictionary.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Controllers;
using Shared.Exceptions;
using Shared.Localization;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/cultures")]
    public class SupportedCultureController : BaseController
    {
        private readonly ISupportedCultureService _service;
        private readonly ISupportedCultureProvider _provider;
        private readonly ILocalizer _localizer;

        public SupportedCultureController(
            ISupportedCultureService service,
            ISupportedCultureProvider provider,
            ILocalizer localizer,
            ILogger<SupportedCultureController> logger)
            : base(logger)
        {
            _service = service;
            _provider = provider;
            _localizer = localizer;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSupportedCultureDto dto)
        {
            try
            {
                var supportedCultureId = await _service.CreateAsync(dto);

                return Ok(supportedCultureId);
            }
            catch (ValidationException e)
            {
                return BadRequest(await _localizer.GetErrorAsync(e.Message));
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured while creating a supported culture.");

                return InternalServerError(e.Message);
            }
        }

        [HttpPost("setAsDefault/{id}")]
        public async Task<IActionResult> SetAsDefault(string id)
        {
            try
            {
                await _service.SetAsDefaultAsync(id);

                return Ok();
            }
            catch (NotFoundException e)
            {
                return NotFound(await _localizer.GetErrorAsync(e.Message));
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured while setting a supported culture as default.");

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
                return NotFound(await _localizer.GetErrorAsync(e.Message));
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured while deleting a supported culture.");

                return InternalServerError(e.Message);
            }
        }

        [HttpGet("dropdown")]
        public async Task<IActionResult> Dropdown()
        {
            try
            {
                return Ok(await _provider.GetDropdownItemsAsync(CultureInfo.CurrentCulture));
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured while getting a culture dropdown options.");

                return InternalServerError(e.Message);
            }
        }
    }
}
