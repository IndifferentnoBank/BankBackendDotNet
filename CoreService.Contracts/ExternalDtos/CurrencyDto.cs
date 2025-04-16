using CoreService.Domain.Enums;

namespace CoreService.Contracts.ExternalDtos;

public class CurrencyDto
{
    public Dictionary<Currency, double> Data { get; set; } = new();
}