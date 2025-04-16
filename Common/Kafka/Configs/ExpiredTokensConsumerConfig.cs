namespace Common.Kafka.Configs;

public class ExpiredTokensConsumerConfig : KafkaConfiguration
{
    public string Topic { get; set; }
    public string GroupId { get; set; }
}