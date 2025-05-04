using System.Diagnostics;
using System.Text.Json;
using Common.Configurations;
using Common.Logging.Dtos;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Common.Logging;

public class KafkaTracingHandler : DelegatingHandler
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _serviceName;

    public KafkaTracingHandler(IProducer<Null, string> producer, IOptions<ServiceInfoConfig> config)
    {
        _producer = producer;
        _serviceName = config.Value.ServiceName;
    }

    private async Task LogRequestAsync(string traceId, string spanId, HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var requestLog = new RequestLogDto
        {
            ServiceName = _serviceName,
            EventType = "http_request_out",
            TraceId = traceId,
            SpanId = spanId,
            ParentSpanId = null,
            Timestamp = DateTime.UtcNow.ToString("o"),
            LogMessage = $"Отправлен HTTP запрос: {request.Method} {request.RequestUri}",
            Data = new RequestData { HttpMethod = request.Method.Method, Url = request.RequestUri.ToString() }
        };

        await _producer.ProduceAsync("monitoring-logs.request",
            new Message<Null, string> { Value = JsonSerializer.Serialize(requestLog) }, cancellationToken);
    }

    private async Task LogResponseAsync(string traceId, string spanId, HttpResponseMessage response, Stopwatch sw,
        CancellationToken cancellationToken)
    {
        var responseLog = new ResponseLogDto
        {
            ServiceName = _serviceName,
            EventType = "http_response_out",
            TraceId = traceId,
            SpanId = spanId,
            DurationMs = (int)sw.ElapsedMilliseconds,
            Timestamp = DateTime.UtcNow.ToString("o"),
            LogMessage = $"Получен ответ: {response.StatusCode}",
            Data = new ResponseData { HttpStatus = (int)response.StatusCode }
        };

        await _producer.ProduceAsync("monitoring-logs.response",
            new Message<Null, string> { Value = JsonSerializer.Serialize(responseLog) }, cancellationToken);
    }

    private async Task LogErrorAsync(string traceId, string spanId, Exception ex, CancellationToken cancellationToken)
    {
        var errorLog = new ErrorLogDto
        {
            ServiceName = _serviceName,
            EventType = "error",
            TraceId = traceId,
            SpanId = spanId,
            Timestamp = DateTime.UtcNow.ToString("o"),
            LogMessage = $"Ошибка при запросе: {ex.Message}"
        };

        await _producer.ProduceAsync("monitoring-logs.errors",
            new Message<Null, string> { Value = JsonSerializer.Serialize(errorLog) }, cancellationToken);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var traceId = Guid.NewGuid().ToString();
        var spanId = Guid.NewGuid().ToString();
        var sw = Stopwatch.StartNew();

        await LogRequestAsync(traceId, spanId, request, cancellationToken);

        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            sw.Stop();

            await LogResponseAsync(traceId, spanId, response, sw, cancellationToken);
            return response;
        }
        catch (Exception ex)
        {
            sw.Stop();
            await LogErrorAsync(traceId, spanId, ex, cancellationToken);
            throw;
        }
    }
}