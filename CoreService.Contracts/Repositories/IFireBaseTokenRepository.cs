using Common.GenericRepository;
using Common.Helpers;
using CoreService.Domain.Entities;

namespace CoreService.Contracts.Repositories;

public interface IFireBaseTokenRepository : IGenericRepository<FireBaseToken>
{
    Task<IReadOnlyList<string>> GetFireBaseTokenByUserIdAsync(Guid userId);
    Task<IReadOnlyList<string>> GetFireBaseTokenByRoleAsync(Roles role = Roles.STAFF);
    Task DeleteRangeOfTokensAsync(List<string> tokens);
}