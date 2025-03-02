using System.Text.Json.Serialization;

namespace UserService.Domain.Enums;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole
{
    CUSTOMER,
    STAFF
}
