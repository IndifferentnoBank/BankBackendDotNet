using System.Text.Json.Serialization;

namespace CoreService.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionType
{
    WITHDRAW, 
    DEPOSIT,
    AUTOPAY_LOAN,
    PAY_LOAN,
    TAKE_LOAN,
    TRANSFER
}