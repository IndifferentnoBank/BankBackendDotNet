using Common.Helpers;
using CoreService.Application.Dtos.Requests;
using MediatR;

namespace CoreService.Application.Features.Commands.CreateTransaction;

public record CreateTransactionCommand(Guid BankAccountId, UserClaims UserClaims, CreateTransactionDto CreateTransactionDto): IRequest<Unit>;