using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Middleware;

public class TracingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TracingMiddleware> _logger;
    private static int _totalRequests = 0;
    private static int _failedRequests = 0;

    public TracingMiddleware(RequestDelegate next, ILogger<TracingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        Interlocked.Increment(ref _totalRequests);
        var requestId = Guid.NewGuid();

        try
        {
            context.Items["RequestId"] = requestId;
            _logger.LogInformation("→ Request {RequestId}: {Method} {Path}", requestId, context.Request.Method,
                context.Request.Path);

            await _next(context);

            sw.Stop();
            _logger.LogInformation("✔ Response {RequestId}: {StatusCode} in {Elapsed} ms",
                requestId, context.Response.StatusCode, sw.ElapsedMilliseconds);

            if (context.Response.StatusCode >= 400)
                Interlocked.Increment(ref _failedRequests);
        }
        catch (Exception ex)
        {
            sw.Stop();
            Interlocked.Increment(ref _failedRequests);

            _logger.LogError(ex, "✖ Exception in request {RequestId}: {Method} {Path} after {Elapsed} ms",
                requestId, context.Request.Method, context.Request.Path, sw.ElapsedMilliseconds);

            throw;
        }

        if (_totalRequests % 100 == 0)
        {
            var errorRate = (_failedRequests / (double)_totalRequests) * 100;
            _logger.LogInformation("📊 Total Requests: {Total}, Errors: {Errors}, Error Rate: {ErrorRate}%",
                _totalRequests, _failedRequests, errorRate.ToString("F2"));
        }
    }
}