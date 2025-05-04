using System.Text.Json.Serialization;

namespace UserService.Infrastructure.Kafka.Events;

public class ExpiredTokenEvent
{
    [JsonPropertyName("deleted_token")] 
    public string DeletedToken { get; set; }
}