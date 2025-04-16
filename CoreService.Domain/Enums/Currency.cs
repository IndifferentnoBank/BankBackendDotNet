using System.Text.Json.Serialization;

namespace CoreService.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Currency
{
    USD,
    EUR,
    RUB,
}