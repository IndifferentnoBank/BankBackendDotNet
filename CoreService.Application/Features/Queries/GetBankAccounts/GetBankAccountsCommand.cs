using Common.Helpers;
using CoreService.Application.Dtos.Responses;
using MediatR;

namespace CoreService.Application.Features.Queries.GetBankAccounts;

public record GetBankAccountsCommand(UserClaims UserClaims, string? BankAccountNumber, string? BankAccountName)
    : IRequest<List<BankAccountDto>>;