using AutoMapper;
using Common.Exceptions;
using CoreService.Application.Dtos.Responses;
using CoreService.Infrastructure.ExternalServices.UserService;
using CoreService.Persistence.Repositories.BankAccountRepository;
using MediatR;

namespace CoreService.Application.Features.Queries.GetBankAccountById;

public class GetBankAccountByIdCommandHandler : IRequestHandler<GetBankAccountByIdCommand, BankAccountDto>
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IBankAccountRepository _repository;

    public GetBankAccountByIdCommandHandler(IMapper mapper, IBankAccountRepository repository, IUserService userService)
    {
        _mapper = mapper;
        _repository = repository;
        _userService = userService;
    }

    public async Task<BankAccountDto> Handle(GetBankAccountByIdCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.CheckIfBankAccountExistsByAccountId(request.Id))
            throw new NotFound("Bank Account Not Found");

        var user = await _userService.GetUserInfoAsync(request.UserId);

        var bankAccount = await _repository.GetByIdAsync(request.Id);
        
        if (user == null)
        {
            throw new Forbidden("User not found.");
        }

        if (user.Id != bankAccount.UserId && user.Role != "STAFF")
        {
            throw new Forbidden("You do not have permission to access this bank account.");
        }

        if (user.IsLocked)
        {
            throw new Forbidden("Your account is locked. Please contact support.");
        }
        return _mapper.Map<BankAccountDto>(bankAccount);
    }
}