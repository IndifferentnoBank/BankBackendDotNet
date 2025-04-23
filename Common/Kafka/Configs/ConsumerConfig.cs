namespace Common.Kafka.Configs;

public class KafkaConsumerConfig : KafkaConfiguration
{
    public string Topic { get; set; }
    public string GroupId { get; set; }
    public bool EnableAutoCommit { get; set; }
    public bool EnableAutoOffsetStore { get; set; }
}