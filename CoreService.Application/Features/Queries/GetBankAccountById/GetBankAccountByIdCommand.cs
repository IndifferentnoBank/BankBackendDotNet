using CoreService.Application.Dtos.Responses;
using MediatR;

namespace CoreService.Application.Features.Queries.GetBankAccountById;

public record GetBankAccountByIdCommand(Guid Id, Guid UserId) : IRequest<BankAccountDto>;