using Common.Exceptions;
using CoreService.Domain.Entities;
using CoreService.Domain.Enums;
using CoreService.Persistence.Repositories.BankAccountRepository;
using CoreService.Persistence.Repositories.TransactionsRepository;

namespace CoreService.Application.Helpers.TransactionExecutor;

public class TransactionExecutor : ITransactionExecutor
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public TransactionExecutor(IBankAccountRepository bankAccountRepository, ITransactionRepository transactionRepository)
    {
        _bankAccountRepository = bankAccountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task ExecuteTransactionAsync(Transaction transaction)
    {
        var bankAccount = transaction.BankAccount;
        
        try
        {
            await ProcessTransaction(transaction, bankAccount);
        }
        catch (Exception)
        {
            transaction.Status = TransactionStatus.Rejected;
            throw new BadRequest("An error occurred while processing the transaction.");
        }

        await _transactionRepository.UpdateAsync(transaction);
        await _bankAccountRepository.UpdateAsync(bankAccount);
    }

    private Task ProcessTransaction(Transaction transaction, BankAccount bankAccount)
    {
        switch (transaction.Type)
        {
            case TransactionType.AUTOPAY_LOAN:
                HandleAutoPayLoan(transaction, bankAccount);
                break;

            case TransactionType.PAY_LOAN:
            case TransactionType.WITHDRAW:
                HandleWithdrawalOrLoanPayment(transaction, bankAccount);
                break;

            case TransactionType.DEPOSIT:
            case TransactionType.TAKE_LOAN:
                HandleDepositOrLoanDisbursement(transaction, bankAccount);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(transaction.Type), transaction.Type, "Invalid transaction type.");
        }

        return Task.CompletedTask;
    }

    private void HandleAutoPayLoan(Transaction transaction, BankAccount bankAccount)
    {
        if (bankAccount.Balance < (decimal)transaction.Amount)
        {
            transaction.Status = TransactionStatus.Rejected;
            return;
        }

        DeductAmountFromBalance(transaction, bankAccount);
        transaction.Status = TransactionStatus.Completed;
    }

    private void HandleWithdrawalOrLoanPayment(Transaction transaction, BankAccount bankAccount)
    {
        DeductAmountFromBalance(transaction, bankAccount);
        transaction.Status = TransactionStatus.Completed;
    }

    private void HandleDepositOrLoanDisbursement(Transaction transaction, BankAccount bankAccount)
    {
        AddAmountToBalance(transaction, bankAccount);
        transaction.Status = TransactionStatus.Completed;
    }

    private static void DeductAmountFromBalance(Transaction transaction, BankAccount bankAccount)
    {
        if (bankAccount.Balance < (decimal)transaction.Amount)
        {
            throw new BadRequest("Insufficient funds for the transaction.");
        }

        bankAccount.Balance -= (decimal)transaction.Amount;
    }

    private static void AddAmountToBalance(Transaction transaction, BankAccount bankAccount)
    {
        bankAccount.Balance += (decimal)transaction.Amount;
    }
}