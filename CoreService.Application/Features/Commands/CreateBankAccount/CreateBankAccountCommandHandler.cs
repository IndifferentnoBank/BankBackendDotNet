using Common.Exceptions;
using CoreService.Contracts.Interfaces;
using CoreService.Contracts.Repositories;
using CoreService.Domain.Entities;
using MediatR;

namespace CoreService.Application.Features.Commands.CreateBankAccount;

public class CreateBankAccountCommandHandler : IRequestHandler<CreateBankAccountCommand, Unit>
{
    private readonly IBankAccountNumberGenerator _bankAccountNumberGenerator;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUserService _userService;

    public CreateBankAccountCommandHandler(IBankAccountNumberGenerator bankAccountNumberGenerator,
        IBankAccountRepository bankAccountRepository, IUserService userService)
    {
        _bankAccountNumberGenerator = bankAccountNumberGenerator;
        _bankAccountRepository = bankAccountRepository;
        _userService = userService;
    }


    public async Task<Unit> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserInfoAsync(request.UserClaims.UserId, request.UserClaims.Token);

        if (user.IsLocked) throw new Forbidden("You are not allowed to create bank account.");


        var bankAccountNumber = await GenerateUniqueBankAccountNumber();

        var bankAccount = new BankAccount(request.UserClaims.UserId, request.CreateBankAccountDto.Name,
            bankAccountNumber, request.CreateBankAccountDto.Currency);

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