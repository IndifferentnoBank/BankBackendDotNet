namespace CoreService.Contracts.Interfaces;

public interface ICommissionService
{
    double GetCurrencyCommission(double amount);
    double GetTransactionCommission(double amount);
}