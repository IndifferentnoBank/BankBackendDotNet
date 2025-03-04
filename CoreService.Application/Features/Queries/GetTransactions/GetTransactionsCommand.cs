using CoreService.Application.Dtos.Responses;
using MediatR;

namespace CoreService.Application.Features.Queries.GetTransactions;

public record GetTransactionsCommand(Guid BankAccountId, Guid UserId): IRequest<List<TransactionDto>>;