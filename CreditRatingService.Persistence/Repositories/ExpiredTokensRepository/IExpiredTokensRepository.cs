using Common.GenericRepository;
using CreditRatingService.Domain.Entities;

namespace CreditRatingService.Persistence.Repositories.ExpiredTokensRepository;

public interface IExpiredTokensRepository : IGenericRepository<ExpiredToken>
{
    Task<bool> CheckIfTokenAlreadyExists(string token);
}