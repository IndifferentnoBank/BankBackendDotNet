using AutoMapper;
using CoreService.Application.Dtos.Responses;
using CoreService.Contracts.Repositories;
using CoreService.Persistence.Repositories.TransactionsRepository;
using MediatR;

namespace CoreService.Application.Features.Queries.GetLoanPayments;

public class GetLoanPaymentsCommandHandler : IRequestHandler<GetLoanPaymentsCommand, List<LoanTransactionDto>>
{
    private IMapper _mapper;
    private readonly ITransactionRepository _transactionRepository;

    public GetLoanPaymentsCommandHandler(ITransactionRepository transactionRepository, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<List<LoanTransactionDto>> Handle(GetLoanPaymentsCommand request,
        CancellationToken cancellationToken)
    {
        //todo: add role check and loan check

        var transactions = await _transactionRepository.FindAsync(x => x.RelatedLoanId == request.LoanId);

        return _mapper.Map<List<LoanTransactionDto>>(transactions);
    }
}