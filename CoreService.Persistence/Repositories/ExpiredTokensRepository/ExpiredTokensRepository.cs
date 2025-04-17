using Common.GenericRepository;
using CoreService.Contracts.Repositories;
using CoreService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreService.Persistence.Repositories.ExpiredTokensRepository;

public class ExpiredTokensRepository : GenericRepository<ExpiredToken>, IExpiredTokensRepository
{
    private readonly CoreServiceDbContext _context;

    public ExpiredTokensRepository(CoreServiceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> CheckIfTokenAlreadyExists(string token)
    {
        return await _context.ExpiredTokens.AnyAsync(x => x.Key == token);
    }
}