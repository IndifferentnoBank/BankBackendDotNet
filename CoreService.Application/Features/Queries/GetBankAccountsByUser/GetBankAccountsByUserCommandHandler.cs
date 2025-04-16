using AutoMapper;
using Common.Exceptions;
using Common.Helpers;
using CoreService.Application.Dtos.Responses;
using CoreService.Contracts.Interfaces;
using CoreService.Contracts.Repositories;
using MediatR;

namespace CoreService.Application.Features.Queries.GetBankAccountsByUser;

public class GetBankAccountsByUserCommandHandler : IRequestHandler<GetBankAccountsByUserCommand, List<BankAccountDto>>
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public GetBankAccountsByUserCommandHandler(IBankAccountRepository bankAccountRepository, IMapper mapper,
        IUserService userService)
    {
        _bankAccountRepository = bankAccountRepository;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<List<BankAccountDto>> Handle(GetBankAccountsByUserCommand request,
        CancellationToken cancellationToken)
    {
        if (!request.UserClaims.Roles.Contains(Roles.STAFF))
        {
            /*var user = await _userService.GetUserInfoAsync(request.UserClaims.UserId);

            if (user.Id != request.ClientId)
                throw new Forbidden("You do not have permission to access this command");*/
        }

        var bankAccounts = await _bankAccountRepository.FindAsync(x => x.UserId == request.ClientId);
        return _mapper.Map<List<BankAccountDto>>(bankAccounts);
    }
}