using API.Exceptions;
using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
    public class CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger, IHostEnvironment env)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<CustomExceptionMiddleware> _logger = logger;
        private readonly IHostEnvironment _env = env;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {

                await _next.Invoke(context);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message,ex.StackTrace)
                    : new ApiException(context.Response.StatusCode,ex.Message,"Internal server error");

                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                string json = JsonSerializer.Serialize(response,options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
