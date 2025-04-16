using Common.GenericRepository;
using CoreService.Contracts.Repositories;
using CoreService.Domain.Entities;

namespace CoreService.Persistence.Repositories.ExpiredTokensRepository;

public class ExpiredTokensRepository : GenericRepository<ExpiredToken>, IExpiredTokensRepository
{
    public ExpiredTokensRepository(CoreServiceDbContext context) : base(context)
    {
    }
}