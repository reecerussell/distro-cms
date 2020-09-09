using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Shared.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> Logger;

        public static JsonSerializerOptions BaseJsonOptions = new JsonSerializerOptions
        {
            AllowTrailingCommas = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = true
        };

        protected JsonSerializerOptions JsonOptions = BaseJsonOptions;

        protected BaseController(
            ILogger<BaseController> logger)
        {
            Logger = logger;
        }

        public override BadRequestObjectResult BadRequest(object error)
        {
            Logger.LogDebug("Creating BadRequest response, with error: {0}", error);

            return new BadRequestObjectResult(CreateResponse(error: error.ToString()));
        }

        public ObjectResult NotFound(string error)
        {
            Logger.LogDebug("Creating NotFound response, with error: {0}", error);

            var responseData = CreateResponse(error: error);
            return new ObjectResult(responseData)
            {
                StatusCode = (int)HttpStatusCode.NotFound
            };
        }

        public override OkObjectResult Ok(object value)
        {
            var json = JsonSerializer.Serialize(value, JsonOptions);
            Logger.LogDebug("Creating OK response, with data:\n{0}", json);

            return new OkObjectResult(CreateResponse(value));
        }

        public ObjectResult InternalServerError(string error)
        {
            Logger.LogDebug("Creating InternalServerError response, with error: {0}", error);

            var responseData = CreateResponse(error: error);
            return new ObjectResult(responseData)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        protected virtual ResponseData CreateResponse(object data = null, string error = null)
        {
            return new ResponseData
            {
                Data = data,
                Error = error
            };
        }
    }
}
