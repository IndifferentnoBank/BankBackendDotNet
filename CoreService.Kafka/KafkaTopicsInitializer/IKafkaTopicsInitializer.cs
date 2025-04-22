namespace CoreService.Kafka.KafkaTopicsInitializer;

public interface IKafkaTopicsInitializer
{
    Task InitializeTopicsAsync();
}