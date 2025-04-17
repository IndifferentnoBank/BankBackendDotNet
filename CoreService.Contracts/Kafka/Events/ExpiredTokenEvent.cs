using System.Text.Json.Serialization;

namespace CoreService.Contracts.Kafka.Events;

public class ExpiredTokenEvent
{
    [JsonPropertyName("deleted_token")] 
    public string DeletedToken { get; set; }
}