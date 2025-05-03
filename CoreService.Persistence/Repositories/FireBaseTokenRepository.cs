using Common.GenericRepository;
using Common.Helpers;
using CoreService.Contracts.Repositories;
using CoreService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreService.Persistence.Repositories;

public class FireBaseTokenRepository : GenericRepository<FireBaseToken>, IFireBaseTokenRepository
{
    private readonly CoreServiceDbContext _dbContext;

    public FireBaseTokenRepository(CoreServiceDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task<IReadOnlyList<string>> GetFireBaseTokenByUserIdAsync(Guid userId)
    {
        return await _dbContext.FireBaseTokens.Where(x => x.UserId == userId).Select(x => x.Token).ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetFireBaseTokenByRoleAsync(Roles role = Roles.STAFF)
    {
        return await _dbContext.FireBaseTokens.Where(x => x.Service == role).Select(x => x.Token).ToListAsync();
    }

    public async Task DeleteRangeOfTokensAsync(List<string> tokenIds)
    {
        var entitiesToRemove = await _dbContext.FireBaseTokens
            .Where(x => tokenIds.Contains(x.Token))
            .ToListAsync();

        _dbContext.FireBaseTokens.RemoveRange(entitiesToRemove);
        await _dbContext.SaveChangesAsync();
    }

}