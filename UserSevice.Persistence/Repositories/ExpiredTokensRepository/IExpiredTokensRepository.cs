using Common.GenericRepository;
using UserService.Domain.Entities;

namespace UserService.Contracts.Repositories;

public interface IExpiredTokensRepository : IGenericRepository<ExpiredToken>
{
    Task<bool> CheckIfTokenAlreadyExists(string token);
}