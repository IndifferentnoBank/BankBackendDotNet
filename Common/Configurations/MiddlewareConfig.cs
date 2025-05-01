using Common.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Common.Configurations;

public static class MiddlewareConfig
{
    public static void UseMiddleware(this WebApplication app)
    {
        app.UseMiddleware<TracingMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}