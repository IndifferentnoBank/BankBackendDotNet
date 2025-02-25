using CoreService.Application.Dtos.Requests;
using MediatR;

namespace CoreService.Application.Features.Commands.CreateBankAccount;

public record CreateBankAccountCommand(CreateBankAccountDto CreateBankAccountDto): IRequest<Unit>;