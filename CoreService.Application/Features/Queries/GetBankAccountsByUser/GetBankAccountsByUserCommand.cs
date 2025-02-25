using CoreService.Application.Dtos.Responses;
using MediatR;

namespace CoreService.Application.Features.Queries.GetBankAccountsByUser;

public record GetBankAccountsByUserCommand(Guid UserId): IRequest<List<BankAccountDto>>;