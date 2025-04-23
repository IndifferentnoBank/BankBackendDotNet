using Common.Helpers;
using CoreService.Application.Dtos.Requests;
using MediatR;

namespace CoreService.Application.Features.Commands.CreateBankAccount;

public record CreateBankAccountCommand(UserClaims UserClaims, CreateBankAccountDto CreateBankAccountDto): IRequest<Unit>;