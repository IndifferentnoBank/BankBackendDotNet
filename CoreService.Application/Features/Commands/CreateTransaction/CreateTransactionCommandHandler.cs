using Common.Exceptions;
using CoreService.Domain.Entities;
using CoreService.Domain.Enums;
using CoreService.Persistence.Repositories.BankAccountRepository;
using CoreService.Persistence.Repositories.TransactionsRepository;
using MediatR;

namespace CoreService.Application.Features.Commands.CreateTransaction;

public class CreateTransactionCommandHandler: IRequestHandler<CreateTransactionCommand, Unit>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBankAccountRepository _bankAccountRepository;


    public CreateTransactionCommandHandler(ITransactionRepository transactionRepository, IBankAccountRepository bankAccountRepository)
    {
        _transactionRepository = transactionRepository;
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<Unit> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
       if(!await _bankAccountRepository.CheckIfBankAccountExistsByAccountId(request.BankAccountId))
           throw new NotFound("Bank Account Not Found");
       
       //todo: check user and permission
       
       var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId);
       
       if(bankAccount.isClosed)
           throw new BadRequest("Bank Account Is Closed");
       
       if (request.CreateTransactionDto.Type is TransactionType.WITHDRAW or TransactionType.PAY_LOAN or TransactionType.AUTOPAY_LOAN)
       {
           if (bankAccount.Balance < Convert.ToDecimal(request.CreateTransactionDto.Amount))
           {
               throw new BadRequest("Insufficient Balance");
           }
       }
       var transaction = new Transaction(request.CreateTransactionDto.Type, request.CreateTransactionDto.Amount, request.CreateTransactionDto.Comment, bankAccount);

       await _transactionRepository.AddAsync(transaction);
       
       return Unit.Value;
    }
}