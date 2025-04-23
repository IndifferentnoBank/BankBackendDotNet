using Common.Helpers;
using CoreService.Application.Dtos.Requests;
using MediatR;

namespace CoreService.Application.Features.Commands.TransferMoney;

public record TransferMoneyCommand(UserClaims UserClaims, TransferMoneyDto TransferMoneyDto) : IRequest<Unit>;