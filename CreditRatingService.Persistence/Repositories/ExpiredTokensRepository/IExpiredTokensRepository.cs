using Common.GenericRepository;
using CreditRatingService.Domain.Entities;

namespace CreditRatingService.Contracts.Repositories;

public interface IExpiredTokensRepository : IGenericRepository<ExpiredToken>
{
    
}