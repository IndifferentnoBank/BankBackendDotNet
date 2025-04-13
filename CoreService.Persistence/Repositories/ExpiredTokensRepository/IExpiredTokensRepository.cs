using Common.GenericRepository;
using CoreService.Domain.Entities;

namespace CoreService.Persistence.Repositories.ExpiredTokensRepository;

public interface IExpiredTokensRepository : IGenericRepository<ExpiredToken>
{
    
}