using Common.Helpers;
using CoreService.Application.Dtos.Responses;
using CoreService.Contracts.ExternalDtos;
using CoreService.Infrastructure.SignalR;
using MediatR;

namespace CoreService.Application.Features.Queries.GetTransactions;

public record GetTransactionsCommand(Guid BankAccountId, UserClaims UserClaims): IRequest<List<TransactionDto>>;