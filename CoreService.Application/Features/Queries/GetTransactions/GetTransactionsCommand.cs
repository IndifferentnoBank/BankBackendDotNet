using Common.Helpers;
using CoreService.Contracts.ExternalDtos;
using MediatR;

namespace CoreService.Application.Features.Queries.GetTransactions;

public record GetTransactionsCommand(Guid BankAccountId, UserClaims UserClaims): IRequest<List<TransactionDto>>;