using Common.GenericRepository;
using CoreService.Contracts.Repositories;
using CoreService.Domain.Entities;

namespace CoreService.Persistence.Repositories;

public class FireBaseTokenRepository : GenericRepository<FireBaseToken>, IFireBaseTokenRepository
{
    public FireBaseTokenRepository(CoreServiceDbContext context) : base(context)
    {
    }
}