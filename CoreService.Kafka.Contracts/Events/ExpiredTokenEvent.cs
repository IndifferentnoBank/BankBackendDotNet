namespace CoreService.Kafka.Contracts.Events;

public class ExpiredTokenEvent
{
    public Guid UserId { get; set; }
    public string Key { get; set; }
    public DateTime ExpirationDate { get; set; }
}