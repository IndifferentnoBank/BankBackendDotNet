using Common.Exceptions;
using CoreService.Contracts.Repositories;
using CoreService.Infrastructure.ExternalServices.UserService;
using MediatR;

namespace CoreService.Application.Features.Commands.CloseBankAccount;

public class CloseBankAccountCommandHandler: IRequestHandler<CloseBankAccountCommand, Unit>
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUserService _userService;

    public CloseBankAccountCommandHandler(IBankAccountRepository bankAccountRepository, IUserService userService)
    {
        _bankAccountRepository = bankAccountRepository;
        _userService = userService;
    }

    public async Task<Unit> Handle(CloseBankAccountCommand request, CancellationToken cancellationToken)
    {
       if(!await _bankAccountRepository.CheckIfBankAccountExistsByAccountId(request.BankAccountId))
           throw new NotFound("Bank account not found");
       
       var user = await _userService.GetUserInfoAsync(request.UserId);
       
       var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId);
       
       if(!await _bankAccountRepository.CheckIfBankAccountBelongsToUserAsync(request.BankAccountId, user.Id))
           throw new Forbidden("This bank account is not belong to this user");

       if(bankAccount.isClosed) throw new BadRequest("This bank account is closed");
       
       bankAccount.isClosed = true;
       await _bankAccountRepository.UpdateAsync(bankAccount);
       
       return Unit.Value;
    }
}