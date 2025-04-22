using Common.Helpers;
using CreditRatingService.Application.Services;
using CreditRatingService.Contracts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CreditRatingService.Presentation.Controllers
{
    [Route("api/CreditRatings")]
    [Authorize(Policy = "CustomPolicy")]
    [ApiController]
    public class CreditRatingController : ControllerBase
    {
        private readonly IOverdueTransactionService _analyzer;
        private readonly ICreditRatingService _creditRatingService;
        public CreditRatingController(IOverdueTransactionService analyzer, ICreditRatingService creditRatingService)
        {
            _analyzer = analyzer;
            _creditRatingService = creditRatingService;
        }

        [HttpGet("{loanId}")]
        public async Task<IActionResult> GetOverdueTransactions(Guid loanId)
        {
            var loans = await _analyzer.AnalyzeLoanPaymentsAsync(loanId, JwtHelper.ExtractUserClaimsFromHeader(HttpContext));
            return Ok(loans);
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCreditRating(Guid userId)
        {
            var loans = await _creditRatingService.CalculateUserRatingAsync(userId, JwtHelper.ExtractUserClaimsFromHeader(HttpContext));
            return Ok(loans);
        }
    }
}
