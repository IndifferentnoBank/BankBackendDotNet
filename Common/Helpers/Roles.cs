using System.Text.Json.Serialization;

namespace Common.Helpers;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Roles
{
    STAFF,
    CUSTOMER
}