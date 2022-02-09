using Totem.Business.Core.DataTransferModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Totem.API.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult FromExecutionResult(ExecutionResult result, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            if (result.Success)
            {
                return Ok(result);
            }

            return HandleHttpStatusCodes(result, statusCode);
        }

        protected IActionResult FromExecutionResult<T>(ExecutionResult<T> result, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            if (result.Success)
            {
                return Ok(result.Value as object ?? result);
            }

            return HandleHttpStatusCodes(result, statusCode);
        }

        /// <summary>
        /// This method is implemented to get DRY from two method FromExecutionResult
        /// </summary>
        private IActionResult HandleHttpStatusCodes(ExecutionResult result, HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.Unauthorized => Unauthorized(result),
                HttpStatusCode.Forbidden => Forbid(),
                _ => BadRequest(result),
            };
        }
    }
}
