using Application.Exceptions;
using System.Net;
using System.Text.Json;


namespace AuthServiceApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger logger)
        {
            context.Response.ContentType = "application/json";

            var statusCode = ex switch 
            {
            KeyNotFoundException => HttpStatusCode.NotFound,
            ArgumentException => HttpStatusCode.BadRequest,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            ConflictException => HttpStatusCode.Conflict,
            _ => HttpStatusCode.InternalServerError
            };

            logger.LogError(ex, "Ocurrió un error:{Message}", ex.Message);
            var response = new
            {
                status=(int)statusCode,
                error=ex.Message
            };
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));

        }

    }
}
