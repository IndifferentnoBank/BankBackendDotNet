using Common.Exceptions;
using CoreService.Domain.Enums;
using CoreService.Contracts.Events;
using CoreService.Contracts.Interfaces;
using CoreService.Contracts.Repositories;
using MediatR;

namespace CoreService.Application.Features.Commands.TransferMoney;

public class TransferMoneyCommandHandler : IRequestHandler<TransferMoneyCommand, Unit>
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionProducer _transactionProducer;
    private readonly IUserService _userService;

    public TransferMoneyCommandHandler(
        IBankAccountRepository bankAccountRepository, ITransactionProducer transactionProducer, IUserService userService)
    {
        _bankAccountRepository = bankAccountRepository;
        _transactionProducer = transactionProducer;
        _userService = userService;
    }

    public async Task<Unit> Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
    {
        if (request.TransferMoneyDto.Amount <= 0)
            throw new BadRequest("Amount must be greater than 0.");

        if (!await _bankAccountRepository.CheckIfBankAccountExistsByAccountId(
                request.TransferMoneyDto.FromBankAccountId))
            throw new NotFound("From bank account not found.");

        if (!await _bankAccountRepository.CheckIfBankAccountExistsByAccountId(request.TransferMoneyDto.ToBankAccountId))
            throw new NotFound("To bank account not found.");

        var fromBankAccount = await _bankAccountRepository.GetByIdAsync(request.TransferMoneyDto.FromBankAccountId);
        var toBankAccount = await _bankAccountRepository.GetByIdAsync(request.TransferMoneyDto.ToBankAccountId);

        if (fromBankAccount.isClosed || toBankAccount.isClosed)
            throw new BadRequest("Bank account is closed.");

        if (fromBankAccount.Balance < (decimal)request.TransferMoneyDto.Amount)
            throw new BadRequest("Insufficient balance.");

        var fromUser = await _userService.GetUserInfoAsync(fromBankAccount.UserId);

        if (fromUser != null && fromUser.IsLocked)
            throw new Forbidden("User is blocked");
        
        var toUser = await _userService.GetUserInfoAsync(toBankAccount.UserId);
        
        if (toUser != null && toUser.IsLocked)
            throw new Forbidden("User is blocked");

        var withdraw = new TransactionEvent
        {
            Date = default,
            Amount = request.TransferMoneyDto.Amount,
            Currency = request.TransferMoneyDto.Currency,
            Comment = request.TransferMoneyDto.Comment,
            Type = TransactionType.WITHDRAW,
            Status = TransactionStatus.Processing,
            BankAccountId = fromBankAccount.Id,
        };

        var deposit = new TransactionEvent
        {
            Date = default,
            Amount = request.TransferMoneyDto.Amount,
            Currency = request.TransferMoneyDto.Currency,
            Comment = request.TransferMoneyDto.Comment,
            Type = TransactionType.DEPOSIT,
            Status = TransactionStatus.Processing,
            BankAccountId = toBankAccount.Id,
        };

        withdraw.RelatedTransactionId = deposit.Id;
        deposit.RelatedTransactionId = withdraw.Id;
        
        await _transactionProducer.ProduceTransactionEventAsync(withdraw);
        await _transactionProducer.ProduceTransactionEventAsync(deposit);
        
        return Unit.Value;
    }
}