using Common.Helpers;
using Common.Exceptions;
using CreditRatingService.Application.Dtos.Pesponses;
using CreditRatingService.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditRatingService.Application.Services
{
    public class CreditRatingService: ICreditRatingService
    {
        private readonly ILoanService _loanService;
        private readonly ICoreService _coreService;
        private readonly IOverdueTransactionService _analyzer;

        public CreditRatingService(ILoanService loanService, ICoreService transactionService, IOverdueTransactionService analyzer)
        {
            _loanService = loanService;
            _coreService = transactionService;
            _analyzer = analyzer;
        }

        public async Task<CreditRatingDto> CalculateUserRatingAsync(Guid userId, UserClaims userClaims)
        {
            if (!userClaims.Roles.Contains(Roles.STAFF))
            {
                throw new Forbidden("You have not role for this action");
            }
            var loans = await _loanService.GetLoansByUserIdAsync(userId);

            int overdueCount = 0;

            foreach (var loan in loans)
            {
                var overduePayments = await _analyzer.AnalyzeLoanPaymentsAsync(loan.Id, userClaims);
                overdueCount += overduePayments.Count(p => p.isOverdue);
            }

            var rating = RateUser(overdueCount);

            return new CreditRatingDto
            {
                UserId = userId,
                TotalLoans = loans.Count,
                TotalOverduePayments = overdueCount,
                Rating = rating
            };
        }

        private string RateUser(int overdueCount)
        {
            return overdueCount switch
            {
                0 => "Отличный",
                <= 2 => "Хороший",
                <= 4 => "Удовлетворительный",
                _ => "Плохой"
            };
        }
    }
}
