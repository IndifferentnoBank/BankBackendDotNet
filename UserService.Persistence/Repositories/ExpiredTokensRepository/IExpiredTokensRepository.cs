using Common.GenericRepository;
using UserService.Domain.Entities;

namespace UserService.Persistence.Repositories.ExpiredTokensRepository;

public interface IExpiredTokensRepository : IGenericRepository<ExpiredToken>
{
    Task<bool> CheckIfTokenAlreadyExists(string token);
}