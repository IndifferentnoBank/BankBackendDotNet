using Common.Exceptions;
using CoreService.Persistence.Repositories.BankAccountRepository;
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
       
       //todo: check user and permission
       
       var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId);
       
       if(!await _bankAccountRepository.CheckIfBankAccountBelongsToUserAsync(request.BankAccountId, request.UserId))
           throw new Forbidden("This bank account is not belong to this user");

       bankAccount.isClosed = true;
       await _bankAccountRepository.UpdateAsync(bankAccount);
       
       return Unit.Value;
    }
}