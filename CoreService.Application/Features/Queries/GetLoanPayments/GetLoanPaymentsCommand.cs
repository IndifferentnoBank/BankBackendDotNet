using Common.Helpers;
using CoreService.Application.Dtos.Responses;
using MediatR;

namespace CoreService.Application.Features.Queries.GetLoanPayments;

public record GetLoanPaymentsCommand(UserClaims UserClaims, Guid LoanId) : IRequest<List<LoanTransactionDto>>;