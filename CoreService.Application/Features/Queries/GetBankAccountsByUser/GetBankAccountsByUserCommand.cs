using CoreService.Application.Dtos.Responses;
using MediatR;

namespace CoreService.Application.Features.Queries.GetBankAccountsByUser;

public record GetBankAccountsByUserCommand(Guid UserId, Guid clientId): IRequest<List<BankAccountDto>>;