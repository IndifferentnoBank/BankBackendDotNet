using AutoMapper;
using Common.Exceptions;
using Common.Helpers;
using CoreService.Application.Dtos.Responses;
using CoreService.Contracts.Interfaces;
using CoreService.Contracts.Repositories;
using CoreService.Infrastructure.SignalR;
using MediatR;

namespace CoreService.Application.Features.Queries.GetTransactions;

public class GetTransactionsCommandHandler : IRequestHandler<GetTransactionsCommand, List<TransactionDto>>
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public GetTransactionsCommandHandler(IBankAccountRepository bankAccountRepository,
        ITransactionRepository transactionRepository, IMapper mapper, IUserService userService)
    {
        _bankAccountRepository = bankAccountRepository;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<List<TransactionDto>> Handle(GetTransactionsCommand request, CancellationToken cancellationToken)
    {
        if (!await _bankAccountRepository.CheckIfBankAccountExistsByAccountId(request.BankAccountId))
            throw new NotFound("Bank Account Not Found");

        if (!request.UserClaims.Roles.Contains(Roles.STAFF))
        {
            if (!await _bankAccountRepository.CheckIfBankAccountBelongsToUserAsync(request.BankAccountId,
                    request.UserClaims.UserId))
                throw new Forbidden("This bank account is not belong to this user");
        }

        var transactions = await _transactionRepository.FindAsync(x => x.BankAccountId == request.BankAccountId);

        return _mapper.Map<List<TransactionDto>>(transactions);
    }
}