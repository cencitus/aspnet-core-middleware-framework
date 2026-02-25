using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace frameworks_pr1.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 1. засекаем
        var stopwatch = Stopwatch.StartNew();

        // 2. передаем дальше
        await _next(context);

        // 3. стоп таймер
        stopwatch.Stop();

        // 4. берем айдишник из контекста если есть
        var requestId = context.Items["X-Request-Id"]?.ToString();

        // 5. логируем
        _logger.LogInformation(
            "Request {Method} {Path} responded {StatusCode} in {Elapsed} ms (RequestId: {RequestId})",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds,
            requestId
        );
    }
}