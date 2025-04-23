using Common.Helpers;
using CreditRatingService.Application.Dtos.Pesponses;

namespace CreditRatingService.Application.Services
{
    public interface IOverdueTransactionService
    {
        Task<List<overdueCreditTransactionDto>> AnalyzeLoanPaymentsAsync(Guid loanId, UserClaims userClaims);
    }
}
