using Common.Helpers;
using CreditRatingService.Application.Dtos.Pesponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditRatingService.Application.Services
{
    public interface ICreditRatingService
    {
        Task<CreditRatingDto> CalculateUserRatingAsync(Guid userId, UserClaims userClaims);
    }
}
