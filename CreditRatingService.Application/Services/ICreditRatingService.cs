using Common.Helpers;
using CreditRatingService.Application.Dtos.Pesponses;

namespace CreditRatingService.Application.Services
{
    public interface ICreditRatingService
    {
        Task<CreditRatingDto> CalculateUserRatingAsync(Guid userId, UserClaims userClaims);
    }
}
