using Common.Helpers;
using MediatR;

namespace CoreService.Application.Features.Commands.SendToken;

public record SendTokenCommand(UserClaims UserClaims, Roles Service, string Token) : IRequest<Unit>;