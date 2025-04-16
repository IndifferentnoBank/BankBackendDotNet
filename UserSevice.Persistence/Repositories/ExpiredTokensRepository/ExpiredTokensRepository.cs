using Common.GenericRepository;
using UserService.Contracts.Repositories;
using UserService.Domain.Entities;
using UserSevice.Persistence;

namespace UserService.Persistence.Repositories.ExpiredTokensRepository;

public class ExpiredTokensRepository : GenericRepository<ExpiredToken>, IExpiredTokensRepository
{
    public ExpiredTokensRepository(UserServiceDbContext context) : base(context)
    {
    }
}