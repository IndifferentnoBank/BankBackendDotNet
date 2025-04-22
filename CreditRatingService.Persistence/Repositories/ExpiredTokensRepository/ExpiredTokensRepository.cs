using Common.GenericRepository;
using CreditRatingService.Persistence;
using CreditRatingService.Contracts.Repositories;
using CreditRatingService.Domain.Entities;

namespace UserService.Persistence.Repositories.ExpiredTokensRepository;

public class ExpiredTokensRepository : GenericRepository<ExpiredToken>, IExpiredTokensRepository
{
    public ExpiredTokensRepository(CreditRatingServiceDbContext context) : base(context)
    {
    }
}