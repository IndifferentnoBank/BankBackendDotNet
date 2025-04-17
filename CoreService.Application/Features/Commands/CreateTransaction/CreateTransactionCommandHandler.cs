using Common.Exceptions;
using CoreService.Contracts.Interfaces;
using CoreService.Contracts.Kafka.Events;
using CoreService.Contracts.Repositories;
using CoreService.Domain.Entities;
using CoreService.Domain.Enums;
using MediatR;

namespace CoreService.Application.Features.Commands.CreateTransaction;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Unit>
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionProducer _transactionProducer;
    private readonly IUserService _userService;


    public CreateTransactionCommandHandler(IBankAccountRepository bankAccountRepository,
        ITransactionProducer transactionProducer, IUserService userService)
    {
        _bankAccountRepository = bankAccountRepository;
        _transactionProducer = transactionProducer;
        _userService = userService;
    }

    public async Task<Unit> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        if (!await _bankAccountRepository.CheckIfBankAccountExistsByAccountId(request.BankAccountId))
            throw new NotFound("Bank Account Not Found");

        /*
        var user = await _userService.GetUserInfoAsync(request.UserId);

        if (user.IsLocked) throw new Forbidden("Account Locked");
        */

        var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId);

        if (bankAccount.UserId != request.UserId) throw new Forbidden("You are not allowed to create this transaction");

        if (bankAccount.isClosed)
            throw new BadRequest("Bank Account Is Closed");

        if (request.CreateTransactionDto.Type is TransactionType.WITHDRAW or TransactionType.PAY_LOAN)
        {
            if (bankAccount.Balance < Convert.ToDecimal(request.CreateTransactionDto.Amount))
            {
                throw new BadRequest("Insufficient Balance");
            }
        }

        if (request.CreateTransactionDto.Type is TransactionType.TAKE_LOAN)
        {
            await CheckMasterAccount(request.CreateTransactionDto.Amount);
        }

        var transactionEvent = new TransactionEvent()
        {
            
            Amount = request.CreateTransactionDto.Amount,
            Currency = bankAccount.Currency,
            Comment = request.CreateTransactionDto.Comment,
            Type = request.CreateTransactionDto.Type,
            Status = TransactionStatus.Processing,
            BankAccountId = bankAccount.Id,
            RelatedTransactionId = null,
            RelatedLoanId = request.CreateTransactionDto.RelatedLoanId
        };

        await _transactionProducer.ProduceTransactionEventAsync(transactionEvent);
        
        return Unit.Value;
    }

    private async Task CheckMasterAccount(double amount)
    {
        var masterAccount = await _bankAccountRepository.GetMasterAccountAsync();

        if (masterAccount.Balance < (decimal)amount)
        {
            throw new BadRequest("Bank does not have enough money to give you a loan");
        }
    }
}