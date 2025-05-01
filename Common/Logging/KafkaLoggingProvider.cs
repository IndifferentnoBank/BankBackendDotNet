namespace Common.Logging;

using Microsoft.Extensions.Logging;
using Confluent.Kafka;

public class KafkaLoggerProvider : ILoggerProvider
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;

    public KafkaLoggerProvider(string brokerList, string topic)
    {
        var config = new ProducerConfig { BootstrapServers = brokerList };
        _producer = new ProducerBuilder<Null, string>(config).Build();
        _topic = topic;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new KafkaLogger(_producer, _topic, categoryName);
    }

    public void Dispose() => _producer.Dispose();
}
