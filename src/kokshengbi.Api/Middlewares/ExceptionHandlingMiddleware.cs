using kokshengbi.Application.Common.Exceptions;
using System.Net;

namespace kokshengbi.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                BusinessException businessException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = exception switch
            {
                BusinessException businessException => new
                {
                    errorCode = businessException.Code,
                    message = businessException.Message,
                    details = businessException.Description
                },
                UnauthorizedAccessException unauthorizedException => new
                {
                    errorCode = context.Response.StatusCode,
                    message = unauthorizedException.Message,
                    details = (string)null
                },
                _ => new
                {
                    errorCode = context.Response.StatusCode,
                    message = exception.Message,
                    details = (string)null
                }
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
