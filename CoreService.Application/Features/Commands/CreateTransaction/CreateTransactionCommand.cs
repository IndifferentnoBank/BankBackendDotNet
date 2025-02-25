using CoreService.Application.Dtos.Requests;
using MediatR;

namespace CoreService.Application.Features.Commands.CreateTransaction;

public record CreateTransactionCommand(Guid BankAccountId, CreateTransactionDto CreateTransactionDto): IRequest<Unit>;