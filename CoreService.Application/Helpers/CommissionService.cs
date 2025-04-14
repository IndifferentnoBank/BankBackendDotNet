using CoreService.Contracts.Interfaces;
using Microsoft.Extensions.Options;

namespace CoreService.Application.Helpers;

public class CommissionService : ICommissionService
{
    private readonly decimal _commissionPercent;

    public CommissionService(IOptions<CommissionSettings> options)
    {
        _commissionPercent = options.Value.Commission;
    }

    public double GetCommission(double amount)
    {
        decimal amountDecimal = (decimal)amount;
        decimal commission = amountDecimal * _commissionPercent;
        return (double)commission;
    }
}