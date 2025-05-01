using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoreService.Kafka.KafkaTopicsInitializer;

public class KafkaTopicsInitialization : IKafkaTopicsInitializer
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<KafkaTopicsInitialization> _logger;

    public KafkaTopicsInitialization(IConfiguration configuration, ILogger<KafkaTopicsInitialization> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task InitializeTopicsAsync()
    {
        var bootstrapServers = _configuration["Kafka:BootstrapServers"];
        var topics = _configuration.GetSection("Kafka:Topics").Get<List<TopicConfig>>();

        using var adminClient =
            new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();
        
        if (topics != null)
            foreach (var topic in topics)
            {
                try
                {
                    var topicSpecification = new TopicSpecification
                    {
                        Name = topic.Name,
                        ReplicationFactor = (short)topic.ReplicationFactor,
                        NumPartitions = topic.NumPartitions
                    };

                    await adminClient.CreateTopicsAsync([topicSpecification]);
                    _logger.LogInformation($"Topic '{topic.Name}' created successfully.");
                }
                catch (CreateTopicsException e)
                {
                    _logger.LogWarning(
                        $"An error occurred creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
                }
            }
    }
}

public class TopicConfig
{
    public string Name { get; set; }
    public int ReplicationFactor { get; set; }
    public int NumPartitions { get; set; }
}