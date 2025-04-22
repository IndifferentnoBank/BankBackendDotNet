using CreditRatingService.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditRatingService.Contracts.Interfaces
{
    public interface ICoreService
    {
        Task<List<LoanTransactionDto>> GetTransactionByLoanIdAsync(Guid loanId, string token);
    }
}
