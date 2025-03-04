using CoreService.Application.Dtos.Responses;
using MediatR;

namespace CoreService.Application.Features.Queries.GetBankAccounts;

public record GetBankAccountsCommand(Guid UserId, string? BankAccountNumber, string? BankAccountName)
    : IRequest<List<BankAccountDto>>;