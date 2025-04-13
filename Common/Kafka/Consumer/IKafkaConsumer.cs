namespace Common.Kafka.Consumer;

public interface IKafkaConsumer
{
    Task ConsumeAsync(CancellationToken cancellationToken);
}