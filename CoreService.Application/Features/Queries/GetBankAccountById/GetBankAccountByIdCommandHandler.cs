using AutoMapper;
using Common.Exceptions;
using CoreService.Application.Dtos.Responses;
using CoreService.Persistence.Repositories.BankAccountRepository;
using MediatR;

namespace CoreService.Application.Features.Queries.GetBankAccountById;

public class GetBankAccountByIdCommandHandler: IRequestHandler<GetBankAccountByIdCommand, BankAccountDto>
{
    private readonly IMapper _mapper;
    private readonly IBankAccountRepository _repository;

    public GetBankAccountByIdCommandHandler(IMapper mapper, IBankAccountRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<BankAccountDto> Handle(GetBankAccountByIdCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.CheckIfBankAccountExistsByAccountId(request.Id))
            throw new NotFound("Bank Account Not Found");
        
        //todo: check user permission
        
        return _mapper.Map<BankAccountDto>(await _repository.GetByIdAsync(request.Id));
    }
}