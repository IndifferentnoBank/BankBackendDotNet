using Common.Configurations;

namespace CoreService.Kafka.Config;

public class BankTransactionsConsumerConfig : KafkaConfiguration
{
    public string Topic { get; set; }
    public string GroupId { get; set; }
}