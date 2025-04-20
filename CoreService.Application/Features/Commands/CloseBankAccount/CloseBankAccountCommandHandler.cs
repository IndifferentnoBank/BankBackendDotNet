using Common.Exceptions;
using CoreService.Contracts.Repositories;
using MediatR;

namespace CoreService.Application.Features.Commands.CloseBankAccount;

public class CloseBankAccountCommandHandler: IRequestHandler<CloseBankAccountCommand, Unit>
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public CloseBankAccountCommandHandler(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<Unit> Handle(CloseBankAccountCommand request, CancellationToken cancellationToken)
    {
       if(!await _bankAccountRepository.CheckIfBankAccountExistsByAccountId(request.BankAccountId))
           throw new NotFound("Bank account not found");
       
       var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId);

       var userId = request.UserId;
       if(!await _bankAccountRepository.CheckIfBankAccountBelongsToUserAsync(request.BankAccountId, userId))
           throw new Forbidden("This bank account is not belong to this user");

       if(bankAccount.isClosed) throw new BadRequest("This bank account is closed");
       
       bankAccount.isClosed = true;
       await _bankAccountRepository.UpdateAsync(bankAccount);
       
       return Unit.Value;
    }
}