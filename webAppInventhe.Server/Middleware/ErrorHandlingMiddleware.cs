using System;
using webAppInventhe.Shared.Exception;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Net;

namespace webAppInventhe.Server.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            _logger = loggerFactory?.CreateLogger<ErrorHandlingMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            int httpRequestCode = ex.GetType().IsAssignableFrom(typeof(BadRequestException)) ? 
                (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.InternalServerError;

            var result = JsonConvert.SerializeObject(new { error = ex.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = httpRequestCode;
            _logger.LogError($"{ex.Message} stack trace: {ex.StackTrace}");

            return context.Response.WriteAsync(result);
        }
    }
}
