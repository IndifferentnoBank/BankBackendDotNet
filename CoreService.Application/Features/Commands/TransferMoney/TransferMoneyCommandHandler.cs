using Common.Exceptions;
using CoreService.Domain.Enums;
using CoreService.Contracts.Interfaces;
using CoreService.Contracts.Kafka.Events;
using CoreService.Contracts.Repositories;
using MediatR;

namespace CoreService.Application.Features.Commands.TransferMoney;

public class TransferMoneyCommandHandler : IRequestHandler<TransferMoneyCommand, Unit>
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionProducer _transactionProducer;
    private readonly IUserService _userService;
    private readonly ICurrencyService _currencyService;
    private readonly ICommissionService _commissionService;

    public TransferMoneyCommandHandler(
        IBankAccountRepository bankAccountRepository, ITransactionProducer transactionProducer,
        IUserService userService, ICurrencyService currencyService, ICommissionService commissionService)
    {
        _bankAccountRepository = bankAccountRepository;
        _transactionProducer = transactionProducer;
        _userService = userService;
        _currencyService = currencyService;
        _commissionService = commissionService;
    }

    public async Task<Unit> Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
    {
        /*var fromUser = await _userService.GetUserInfoAsync(request.UserId);

        if (fromUser != null && fromUser.IsLocked)
            throw new Forbidden("User is blocked");*/

        var amount = request.TransferMoneyDto.Amount;

        if (request.TransferMoneyDto.Amount <= 0)
            throw new BadRequest("Amount must be greater than 0.");

        if (!await _bankAccountRepository.CheckIfBankAccountExistsByAccountId(
                request.TransferMoneyDto.FromBankAccountId))
            throw new NotFound("From bank account not found.");

        if (!await _bankAccountRepository.CheckIfBankAccountExistsByAccountId(request.TransferMoneyDto.ToBankAccountId))
            throw new NotFound("To bank account not found.");

        var fromBankAccount = await _bankAccountRepository.GetByIdAsync(request.TransferMoneyDto.FromBankAccountId);
        var toBankAccount = await _bankAccountRepository.GetByIdAsync(request.TransferMoneyDto.ToBankAccountId);

        if (fromBankAccount.UserId != request.UserId)
            throw new Forbidden("You are not allowed to transfer money from this bank account.");

        if (fromBankAccount.isClosed || toBankAccount.isClosed)
            throw new BadRequest("Bank account is closed.");

        if (fromBankAccount.Balance < (decimal)request.TransferMoneyDto.Amount)
            throw new BadRequest("Insufficient balance.");

        var amountToWithdraw = amount;
        var amountToDeposit = amount;
        double commission = 0;


        if (fromBankAccount.Currency != request.TransferMoneyDto.Currency ||
            toBankAccount.Currency != request.TransferMoneyDto.Currency)
        {
            commission = _commissionService.GetCommission(amount);

            amountToWithdraw = await _currencyService.ConvertCurrency(amount + commission,
                request.TransferMoneyDto.Currency, fromBankAccount.Currency);

            if (fromBankAccount.Balance < Convert.ToDecimal(amountToWithdraw))
            {
                throw new BadRequest("Insufficient Balance");
            }

            commission = await _currencyService.ConvertCurrency(commission, fromBankAccount.Currency,
                Currency.RUB);
        }

        if (toBankAccount.Currency != request.TransferMoneyDto.Currency)
        {
            amountToDeposit = await _currencyService.ConvertCurrency(amount, request.TransferMoneyDto.Currency,
                toBankAccount.Currency);
        }

/*
        var toUser = await _userService.GetUserInfoAsync(toBankAccount.UserId);

        if (toUser != null && toUser.IsLocked)
            throw new Forbidden("User is blocked");*/

        var withdraw = new TransactionEvent
        {
            Date = default,
            Amount = amountToWithdraw,
            Currency = fromBankAccount.Currency,
            Comment = request.TransferMoneyDto.Comment,
            Type = TransactionType.WITHDRAW,
            Status = TransactionStatus.Processing,
            BankAccountId = fromBankAccount.Id,
        };

        var deposit = new TransactionEvent
        {
            Date = default,
            Amount = amountToDeposit,
            Currency = toBankAccount.Currency,
            Comment = request.TransferMoneyDto.Comment,
            Type = TransactionType.DEPOSIT,
            Status = TransactionStatus.Processing,
            BankAccountId = toBankAccount.Id,
        };

        withdraw.RelatedTransactionId = deposit.Id;
        deposit.RelatedTransactionId = withdraw.Id;


        await _transactionProducer.ProduceTransactionEventAsync(withdraw);

        if (commission > 0)
        {
            var commissionTransaction = new TransactionEvent
            {
                Date = default,
                Amount = commission,
                Currency = Currency.RUB,
                Comment = "Commission",
                Type = TransactionType.DEPOSIT,
                Status = TransactionStatus.Processing,
                BankAccountId = await _bankAccountRepository.GetMasterAccountIdAsync(),
            };

            await _transactionProducer.ProduceTransactionEventAsync(commissionTransaction);
        }

        await _transactionProducer.ProduceTransactionEventAsync(deposit);

        return Unit.Value;
    }
}