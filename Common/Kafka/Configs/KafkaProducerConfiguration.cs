namespace Common.Kafka.Configs;

public class KafkaProducerConfiguration
{
    public required string Topic { get; set; }
    public required string BootstrapServers { get; set; }
}