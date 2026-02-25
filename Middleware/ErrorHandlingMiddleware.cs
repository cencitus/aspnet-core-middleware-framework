using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace frameworks_pr1.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
            var requestId = context.Items["X-Request-Id"]?.ToString();

            _logger.LogError(ex, "Unhandled exception (RequestId: {RequestId})", requestId);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var body = new
            {
                requestId,
                error = new
                {
                    code = context.Response.StatusCode,
                    message = "Internal server error"
                }
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(body));
        }
    }
}