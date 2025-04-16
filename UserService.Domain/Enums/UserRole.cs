using System.Text.Json.Serialization;

namespace UserService.Domain.Enums;
[JsonConverter(typeof(JsonStringEnumConverter))]

[Flags]
public enum UserRole
{
    CUSTOMER=1,
    STAFF=2
}
