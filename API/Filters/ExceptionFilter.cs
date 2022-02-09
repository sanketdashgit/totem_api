using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Totem.API.Filters
{
    /// <summary>
    /// global error handler returns generic error info to event log and return internal server error to user with error message
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
             response.Headers.Add("Exception", exception.Message);
            response.WriteAsync(exception.Message);
        }
    }
}
