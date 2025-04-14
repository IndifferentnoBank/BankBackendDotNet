using CoreService.Application.Dtos.Responses;
using MediatR;

namespace CoreService.Application.Features.Queries.GetLoanPayments;

public record GetLoanPaymentsCommand(Guid UserId, Guid LoanId): IRequest<IList<LoanTransactionDto>>, IRequest, IRequest<List<LoanTransactionDto>>;