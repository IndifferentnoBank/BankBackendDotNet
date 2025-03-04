using AutoMapper;
using Common.Exceptions;
using CoreService.Application.Dtos.Responses;
using CoreService.Domain.Entities;
using CoreService.Infrastructure.ExternalServices.UserService;
using CoreService.Persistence.Repositories.BankAccountRepository;
using MediatR;

namespace CoreService.Application.Features.Queries.GetBankAccounts;

public class GetBankAccountsCommandHandler : IRequestHandler<GetBankAccountsCommand, List<BankAccountDto>>
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IBankAccountRepository _bankAccountRepository;

    public GetBankAccountsCommandHandler(IMapper mapper, IBankAccountRepository bankAccountRepository, IUserService userService)
    {
        _mapper = mapper;
        _bankAccountRepository = bankAccountRepository;
        _userService = userService;
    }

    public async Task<List<BankAccountDto>> Handle(GetBankAccountsCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserInfoAsync(request.UserId);
        
        if (user.Role != "STAFF") throw new Forbidden("You do not have permission");

        var bankAccounts = await _bankAccountRepository.GetAllBankAccountsAsync();

        var result = ApplyFilters(bankAccounts, request.BankAccountNumber, request.BankAccountName);
        return _mapper.Map<List<BankAccountDto>>(result);
    }

    private List<BankAccount> ApplyFilters(IQueryable<BankAccount> bankAccounts, string? accountNumber,
        string? accountName)
    {
        if (accountNumber != null)
            bankAccounts = bankAccounts.Where(a => a.AccountNumber.Contains(accountNumber));

        if (accountName != null)
            bankAccounts = bankAccounts.Where(a => a.Name.Contains(accountName));


        return bankAccounts.ToList();
    }
}