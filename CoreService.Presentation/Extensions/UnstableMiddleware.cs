using System.Text.Json;

namespace CoreService.Presentation.Extensions;

public class UnstableMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Random _random;

    public UnstableMiddleware(RequestDelegate next)
    {
        _next = next;
        _random = new Random();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var currentMinute = DateTime.UtcNow.Minute;
        var errorChance = currentMinute % 2 == 0 ? 0.9 : 0.5; 
        
        if (_random.NextDouble() < errorChance)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = new { message = "Simulated error" };
            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
            return;
        }

        await _next(context);
    }
    
}