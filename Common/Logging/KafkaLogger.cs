using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Common.Logging;

public class KafkaLogger : ILogger
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;
    private readonly string _category;

    public KafkaLogger(IProducer<Null, string> producer, string topic, string category)
    {
        _producer = producer;
        _topic = topic;
        _category = category;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel, EventId eventId,
        TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        var message = formatter(state, exception);
        var logEntry = $"[{logLevel}] {_category}: {message}";
        _producer.Produce(_topic, new Message<Null, string> { Value = logEntry });
    }
}
