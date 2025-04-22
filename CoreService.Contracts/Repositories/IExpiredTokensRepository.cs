using Common.GenericRepository;
using CoreService.Domain.Entities;

namespace CoreService.Contracts.Repositories;

public interface IExpiredTokensRepository : IGenericRepository<ExpiredToken>
{
    Task<bool> CheckIfTokenAlreadyExists(string token);
}