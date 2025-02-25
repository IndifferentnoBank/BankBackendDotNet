using CoreService.Domain.Entities;
using CoreService.Domain.Enums;
using CoreService.Persistence.Repositories.BankAccountRepository;

namespace CoreService.Application.Helpers.TransactionExecutor;

public class TransactionExecutor: ITransactionExecutor
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public TransactionExecutor(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task ExecuteTransaction(Transaction transaction)
    {
        var bankAccount = transaction.BankAccount;
        
        switch (transaction.Type)
        {
            case TransactionType.PAY_LOAN:
            case TransactionType.WITHDRAW:
            case TransactionType.AUTOPAY_LOAN:
                bankAccount.Balance -= (decimal)transaction.Amount;
                break;

            case TransactionType.DEPOSIT:
            case TransactionType.TAKE_LOAN:
                bankAccount.Balance += (decimal)transaction.Amount; 
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        await _bankAccountRepository.UpdateAsync(bankAccount);
    }
}