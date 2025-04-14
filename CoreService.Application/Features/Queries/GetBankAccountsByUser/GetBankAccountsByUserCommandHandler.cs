using AutoMapper;
using Common.Exceptions;
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
        var user = await _userService.GetUserInfoAsync(request.UserId);

        if (user.Id != request.clientId && user.Role != "STAFF")
            throw new Forbidden("You do not have permission to access this command");

        var bankAccounts = await _bankAccountRepository.FindAsync(x => x.UserId == request.clientId);
        return _mapper.Map<List<BankAccountDto>>(bankAccounts);
    }
}