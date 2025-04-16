using Common.Helpers;
using CoreService.Application.Dtos.Responses;
using MediatR;

namespace CoreService.Application.Features.Queries.GetBankAccountsByUser;

public record GetBankAccountsByUserCommand(UserClaims UserClaims, Guid ClientId): IRequest<List<BankAccountDto>>;