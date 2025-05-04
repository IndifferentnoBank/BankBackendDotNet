using System.Diagnostics;
using System.Text.Json;
using Common.Configurations;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Common.Middleware;

public class TracingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IProducer<Null, string> _producer;
    private readonly string _serviceName; 

    public TracingMiddleware(RequestDelegate next, IProducer<Null, string> producer, IOptions<ServiceInfoConfig> config)
    {
        _next = next;
        _producer = producer;
        _serviceName = config.Value.ServiceName;
    }

    public async Task Invoke(HttpContext context)
    {
        var traceId = context.TraceIdentifier;
        var spanId = Guid.NewGuid().ToString();
        var timestamp = DateTime.UtcNow.ToString("o");
        var method = context.Request.Method;
        var url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";
        var sw = Stopwatch.StartNew();

        var requestLog = new
        {
            service_name = _serviceName,
            event_type = "http_request_in",
            trace_id = traceId,
            span_id = spanId,
            parent_span_id = (string)null,
            timestamp,
            log_message = $"Получен HTTP запрос: {method} {url}",
            data = new { http_method = method, url }
        };
        await _producer.ProduceAsync("monitoring-logs.request", new Message<Null, string> { Value = JsonSerializer.Serialize(requestLog) });

        try
        {
            await _next(context);
            sw.Stop();

            var responseLog = new
            {
                service_name = _serviceName,
                event_type = "http_response_in",
                trace_id = traceId,
                span_id = spanId,
                duration_ms = (int)sw.ElapsedMilliseconds,
                timestamp = DateTime.UtcNow.ToString("o"),
                log_message = $"Ответ отправлен: {context.Response.StatusCode}",
                data = new { http_status = context.Response.StatusCode }
            };
            await _producer.ProduceAsync("monitoring-logs.response", new Message<Null, string> { Value = JsonSerializer.Serialize(responseLog) });
        }
        catch (Exception ex)
        {
            sw.Stop();
            var errorLog = new
            {
                service_name = _serviceName,
                event_type = "error",
                trace_id = traceId,
                span_id = spanId,
                timestamp = DateTime.UtcNow.ToString("o"),
                log_message = $"Ошибка при обработке входящего запроса: {ex.Message}"
            };
            await _producer.ProduceAsync("monitoring-logs.errors", new Message<Null, string> { Value = JsonSerializer.Serialize(errorLog) });
            throw;
        }
    }
}