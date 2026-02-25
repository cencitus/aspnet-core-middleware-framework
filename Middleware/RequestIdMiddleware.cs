using Microsoft.AspNetCore.Http;

namespace frameworks_pr1.Middleware;

public class RequestIdMiddleware
{
    private const string HeaderName = "X-Request-Id";
    private readonly RequestDelegate _next;

    public RequestIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 1. берем айдишник из входящего заголовка
        if (!context.Request.Headers.TryGetValue(HeaderName, out var requestId))
        {
            // 2. не получилось - генерируем новый
            requestId = Guid.NewGuid().ToString();
        }

        // 3.  requestId в HttpContext
        context.Items[HeaderName] = requestId;

        // 4. добавляем в ответ
        context.Response.Headers[HeaderName] = requestId;

        // 5. передаем управление дальше по конвейеру
        await _next(context);
    }
}