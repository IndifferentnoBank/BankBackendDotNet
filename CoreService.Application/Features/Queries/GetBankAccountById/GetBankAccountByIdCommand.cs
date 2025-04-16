using Common.Helpers;
using CoreService.Application.Dtos.Responses;
using MediatR;

namespace CoreService.Application.Features.Queries.GetBankAccountById;

public record GetBankAccountByIdCommand(Guid Id, UserClaims UserClaims) : IRequest<BankAccountDto>;