﻿using Microsoft.AspNetCore.Mvc;
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

        protected IActionResult BadRequest(string error)
        {
            Logger.LogDebug("Creating BadRequest response, with error: {0}", error);

            return CreateResponse(HttpStatusCode.BadRequest, error: error);
        }

        protected ObjectResult NotFound(string error)
        {
            Logger.LogDebug("Creating NotFound response, with error: {0}", error);

            return CreateResponse(HttpStatusCode.NotFound, error: error);
        }

        public override OkObjectResult Ok(object value)
        {
            var json = JsonSerializer.Serialize(value, JsonOptions);
            Logger.LogDebug("Creating OK response, with data:\n{0}", json);

            return CreateResponse(HttpStatusCode.OK, value);
        }

        protected new OkObjectResult Ok()
        {
            Logger.LogDebug("Creating OK response, no data");

            return CreateResponse(HttpStatusCode.OK);
        }

        protected IActionResult InternalServerError(string error)
        {
            Logger.LogDebug("Creating InternalServerError response, with error: {0}", error);

            return CreateResponse(HttpStatusCode.InternalServerError, error: error);
        }

        protected virtual OkObjectResult CreateResponse(HttpStatusCode status, object data = null, string error = null)
        {
            return base.Ok(new ResponseData
            {
                StatusCode = (int)status,
                Data = data,
                Error = error
            });
        }
    }
}
