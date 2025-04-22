using CreditRatingService.Contracts.Interfaces;
using CreditRatingService.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreditRatingService.Application.Dtos.Pesponses;
using Common.Helpers;

namespace CreditRatingService.Application.Services
{
    public class OverdueTransactionService: IOverdueTransactionService
    {
        private readonly ILoanService _loanService;
        private readonly ICoreService _coreService;

        public OverdueTransactionService(ILoanService loanService, ICoreService coreService)
        {
            _loanService = loanService;
            _coreService = coreService;
        }

        public async Task<List<overdueCreditTransactionDto>> AnalyzeLoanPaymentsAsync(Guid loanId, UserClaims userClaims)
        {
            var loan = await _loanService.GetLoanByLoanIdAsync(loanId);
            var transactions = await _coreService.GetTransactionByLoanIdAsync(loanId, userClaims.Token);

            var plannedPayments = GeneratePlannedPayments(loan.StartDate, loan.EndDate, loan.MonthlyPayment, loan.Id);
            var actualPayments = transactions
                .Where(t =>
                    (t.Type == TransactionType.PAY_LOAN || t.Type == TransactionType.AUTOPAY_LOAN) &&
                    t.Status == TransactionStatus.Completed)
                .OrderBy(t => t.Date)
                .ToList();

            var overdueList = new List<overdueCreditTransactionDto>();

            foreach (var planned in plannedPayments)
            {
                var matchingPayment = actualPayments
                    .FirstOrDefault(p =>
                        p.Date.Date <= planned.Date.Date &&
                        Math.Abs(p.Amount - planned.Amount) < 0.01 &&
                        p.RelatedLoanId == planned.RelatedLoanId);

                overdueList.Add(new overdueCreditTransactionDto
                {
                    Id = matchingPayment.Id,
                    isOverdue = matchingPayment == null,
                    Date = planned.Date,
                    Amount = planned.Amount,
                    Type = matchingPayment?.Type ?? TransactionType.PAY_LOAN,
                    Status = matchingPayment?.Status ?? TransactionStatus.Processing,
                    RelatedLoanId = planned.RelatedLoanId
                });

                if (matchingPayment != null)
                    actualPayments.Remove(matchingPayment);
            }

            return overdueList;
        }

        private List<overdueCreditTransactionDto> GeneratePlannedPayments(DateTime startDate, DateTime endDate, double monthlyPayment, Guid loanId)
        {
            var result = new List<overdueCreditTransactionDto>();
            var current = new DateTime(startDate.Year, startDate.Month, startDate.Day);

            while (current <= endDate)
            {
                result.Add(new overdueCreditTransactionDto
                {
                    Id = Guid.NewGuid(),
                    isOverdue = false,
                    Date = current,
                    Amount = monthlyPayment,
                    Type = TransactionType.PAY_LOAN,
                    Status = TransactionStatus.Processing,
                    RelatedLoanId = loanId
                });

                current = current.AddMonths(1);
            }

            return result;
        }
    }
}
