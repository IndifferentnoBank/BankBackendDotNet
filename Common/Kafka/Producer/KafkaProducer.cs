using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;


namespace Common.Kafka.Producer;

public class KafkaProducer<T> : IKafkaProducer<T> where T : class
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducer(IConfiguration configuration)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"]
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task ProduceAsync(string topic, string key, T message, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(message);
        var kafkaMessage = new Message<string, string> { Key = key, Value = json };

        try
        {
            await _producer.ProduceAsync(topic, kafkaMessage, cancellationToken);
            Console.WriteLine($"Message sent to topic '{topic}' with key: {key}");
        }
        catch (ProduceException<string, string> ex)
        {
            Console.WriteLine($"Error producing message: {ex.Error.Reason}");
        }
    }
}