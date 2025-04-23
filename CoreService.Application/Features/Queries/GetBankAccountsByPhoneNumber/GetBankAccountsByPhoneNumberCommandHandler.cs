using CoreService.Application.Dtos.Responses;
using CoreService.Contracts.Interfaces;
using CoreService.Contracts.Repositories;
using MediatR;

namespace CoreService.Application.Features.Queries.GetBankAccountsByPhoneNumber;

public class
    GetBankAccountsByPhoneNumberCommandHandler : IRequestHandler<GetBankAccountsByPhoneNumberCommand,
    List<ShortenBankAccountDto>>
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUserService _userService;

    public GetBankAccountsByPhoneNumberCommandHandler(IBankAccountRepository bankAccountRepository,
        IUserService userService)
    {
        _bankAccountRepository = bankAccountRepository;
        _userService = userService;
    }

    public async Task<List<ShortenBankAccountDto>> Handle(GetBankAccountsByPhoneNumberCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByPhoneNumberAsync(request.PhoneNumber);
        
        var bankAccounts = await _bankAccountRepository.GetAllAccountsByUserIdAsync(user.Id);
        
        return bankAccounts.Select(bankAccount => new ShortenBankAccountDto
        {
            Id = bankAccount.Id,
            Name = bankAccount.Name,
            AccountNumber = bankAccount.AccountNumber,
            Currency = bankAccount.Currency,
            User = user
        }).ToList();
    }
}