using MediatR;

namespace CoreService.Application.Features.Commands.CloseBankAccount;

public record CloseBankAccountCommand(Guid BankAccountId, Guid UserId): IRequest<Unit>;