using CoreService.Application.Helpers;
using CoreService.Application.Helpers.BankAccountNumberGenerator;
using CoreService.Domain.Entities;
using CoreService.Persistence.Repositories.BankAccountRepository;
using MediatR;

namespace CoreService.Application.Features.Commands.CreateBankAccount;

public class CreateBankAccountCommandHandler : IRequestHandler<CreateBankAccountCommand, Unit>
{
    private readonly IBankAccountNumberGenerator _bankAccountNumberGenerator;
    private readonly IBankAccountRepository _bankAccountRepository;

    public CreateBankAccountCommandHandler(IBankAccountNumberGenerator bankAccountNumberGenerator, IBankAccountRepository bankAccountRepository)
    {
        _bankAccountNumberGenerator = bankAccountNumberGenerator;
        _bankAccountRepository = bankAccountRepository;
    }


    public async Task<Unit> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
    {
        //todo: check if user exists and not blocked
        //think of adding some more checkers

        var bankAccountNumber = await GenerateUniqueBankAccountNumber();

        var bankAccount = new BankAccount(request.CreateBankAccountDto.UserId, request.CreateBankAccountDto.Name,
            bankAccountNumber);

        await _bankAccountRepository.AddAsync(bankAccount);

        return Unit.Value;
    }

    private async Task<string> GenerateUniqueBankAccountNumber()
    {
        string bankAccountNumber;

        do
        {
            bankAccountNumber = _bankAccountNumberGenerator.GenerateBankAccountNumber();
        } while (await _bankAccountRepository.CheckIfBankAccountExistsByAccountNumberAsync(bankAccountNumber));

        return bankAccountNumber;
    }
}