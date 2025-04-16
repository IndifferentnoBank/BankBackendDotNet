using Common.Exceptions;
using CoreService.Contracts.ExternalDtos;
using CoreService.Contracts.Interfaces;
using CoreService.Contracts.Repositories;
using CoreService.Domain.Entities;
using CoreService.Domain.Enums;

namespace CoreService.Application.Helpers;

public class TransactionExecutor : ITransactionExecutor
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionHub _transactionHub;

    public TransactionExecutor(IBankAccountRepository bankAccountRepository,
        ITransactionRepository transactionRepository, ITransactionHub transactionHub)
    {
        _bankAccountRepository = bankAccountRepository;
        _transactionRepository = transactionRepository;
        _transactionHub = transactionHub;
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

        var transactionDto = new TransactionDto
        {
            Id = transaction.Id,
            Date = transaction.Date,
            Amount = transaction.Amount,
            Comment = transaction.Comment,
            Type = transaction.Type,
            Status = transaction.Status
        };

        await _transactionHub.SendTransactionUpdate(transactionDto);
        await _transactionHub.SendTransactionUpdateToBankAccount(bankAccount.Id, transactionDto);
    }

    private async Task ProcessTransaction(Transaction transaction, BankAccount bankAccount)
    {
        switch (transaction.Type)
        {
            case TransactionType.AUTOPAY_LOAN:
            case TransactionType.PAY_LOAN:
                await HandleLoanPayment(transaction, bankAccount);
                break;

            case TransactionType.WITHDRAW:
                HandleWithdrawal(transaction, bankAccount);
                break;

            case TransactionType.DEPOSIT:
                HandleDeposit(transaction, bankAccount);
                break;
            case TransactionType.TAKE_LOAN:
                await HandleLoanDisbursement(transaction, bankAccount);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(transaction.Type), transaction.Type,
                    "Invalid transaction type.");
        }
    }

    private void HandleWithdrawal(Transaction transaction, BankAccount bankAccount)
    {
        DeductAmountFromBalance(transaction, bankAccount);
        transaction.Status = TransactionStatus.Completed;
    }

    private void HandleDeposit(Transaction transaction, BankAccount bankAccount)
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

    private async Task HandleLoanPayment(Transaction transaction, BankAccount bankAccount)
    {
        var masterAccount = await _bankAccountRepository.GetMasterAccountAsync();

        if (bankAccount.Balance < (decimal)transaction.Amount)
        {
            transaction.Status = TransactionStatus.Rejected;
            return;
        }

        DeductAmountFromBalance(transaction, bankAccount);
        AddAmountToBalance(transaction, masterAccount);
        transaction.Status = TransactionStatus.Completed;
    }

    private async Task HandleLoanDisbursement(Transaction transaction, BankAccount bankAccount)
    {
        var masterAccount = await _bankAccountRepository.GetMasterAccountAsync();

        if (masterAccount.Balance < (decimal)transaction.Amount)
        {
            transaction.Status = TransactionStatus.Rejected;
            return;
        }

        DeductAmountFromBalance(transaction, masterAccount);
        AddAmountToBalance(transaction, bankAccount);
        transaction.Status = TransactionStatus.Completed;
    }
}