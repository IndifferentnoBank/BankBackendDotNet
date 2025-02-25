using CoreService.Application.Dtos.Requests;
using MediatR;

namespace CoreService.Application.Features.Commands.CreateBankAccount;

public record CreateBankAccountCommand(Guid UserId, CreateBankAccountDto CreateBankAccountDto): IRequest<Unit>;