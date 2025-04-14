using CoreService.Domain.Enums;

namespace CoreService.Contracts.Interfaces;

public interface ICurrencyService
{
    Task<double> ConvertCurrency(double amount, Currency fromCurrency, Currency toCurrency);
}