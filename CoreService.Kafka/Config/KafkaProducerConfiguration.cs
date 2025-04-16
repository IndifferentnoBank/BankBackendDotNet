namespace CoreService.Kafka.Config;

public class KafkaProducerConfiguration
{
    public string Topic { get; set; }
    public string BootstrapServers { get; set; }
}