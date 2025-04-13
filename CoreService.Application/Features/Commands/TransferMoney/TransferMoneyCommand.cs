using CoreService.Application.Dtos.Requests;
using MediatR;

namespace CoreService.Application.Features.Commands.TransferMoney;

public record TransferMoneyCommand(Guid UserId, TransferMoneyDto TransferMoneyDto) : IRequest<Unit>;