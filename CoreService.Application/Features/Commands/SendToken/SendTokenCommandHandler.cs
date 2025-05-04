using CoreService.Contracts.Interfaces;
using CoreService.Contracts.Repositories;
using CoreService.Domain.Entities;
using MediatR;

namespace CoreService.Application.Features.Commands.SendToken;

public class SendTokenCommandHandler : IRequestHandler<SendTokenCommand, Unit>
{
    private readonly IUserService _userService;
    private readonly IFireBaseTokenRepository _fireBaseTokenRepository;

    public SendTokenCommandHandler(IUserService userService, IFireBaseTokenRepository fireBaseTokenRepository)
    {
        _userService = userService;
        _fireBaseTokenRepository = fireBaseTokenRepository;
    }

    public async Task<Unit> Handle(SendTokenCommand request, CancellationToken cancellationToken)
    {
        //var user = await _userService.GetUserInfoAsync(request.UserClaims.UserId, request.UserClaims.Token);

        await _fireBaseTokenRepository.AddAsync(new FireBaseToken()
        {
            UserId = request.UserClaims.UserId,
            Token = request.Token,
        });

        return Unit.Value;
    }
}