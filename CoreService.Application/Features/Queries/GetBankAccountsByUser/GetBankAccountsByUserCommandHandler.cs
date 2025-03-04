using AutoMapper;
using CoreService.Application.Dtos.Responses;
using CoreService.Persistence.Repositories.BankAccountRepository;
using MediatR;

namespace CoreService.Application.Features.Queries.GetBankAccountsByUser;

public class GetBankAccountsByUserCommandHandler: IRequestHandler<GetBankAccountsByUserCommand, List<BankAccountDto>>
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IMapper _mapper;

    public GetBankAccountsByUserCommandHandler(IBankAccountRepository bankAccountRepository, IMapper mapper)
    {
        _bankAccountRepository = bankAccountRepository;
        _mapper = mapper;
    }

    public async Task<List<BankAccountDto>> Handle(GetBankAccountsByUserCommand request, CancellationToken cancellationToken)
    {
        //todo: check permission
        var bankAccounts = await _bankAccountRepository.FindAsync(x=>x.UserId == request.UserId);
        return _mapper.Map<List<BankAccountDto>>(bankAccounts);
    }
}