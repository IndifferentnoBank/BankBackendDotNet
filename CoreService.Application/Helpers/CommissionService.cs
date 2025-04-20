using CoreService.Contracts.Interfaces;
using Microsoft.Extensions.Options;

namespace CoreService.Application.Helpers;

public class CommissionService : ICommissionService
{
    private readonly decimal _commissionPercent;
    private readonly decimal _maxTransactionMoney;
    private readonly decimal _transactionCommission;

    public CommissionService(IOptions<CommissionSettings> options)
    {
        _maxTransactionMoney = options.Value.MaxTransactionMoney;
        _commissionPercent = options.Value.CurrencyCommission;
        _transactionCommission = options.Value.TransactionCommission;
    }

    public double GetCurrencyCommission(double amount)
    {
        decimal amountDecimal = (decimal)amount;
        decimal commission = amountDecimal * _commissionPercent;
        return (double)commission;
    }

    public double GetTransactionCommission(double amount)
    {
        return (amount > (double)_maxTransactionMoney ? amount * (double)_transactionCommission : 0);
    }
}