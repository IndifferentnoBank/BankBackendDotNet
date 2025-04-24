using Common.GenericRepository;
using CreditRatingService.Persistence;
using CreditRatingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditRatingService.Persistence.Repositories.ExpiredTokensRepository;

public class ExpiredTokensRepository : GenericRepository<ExpiredToken>, IExpiredTokensRepository
{
    private readonly CreditRatingServiceDbContext _context;
    public ExpiredTokensRepository(CreditRatingServiceDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<bool> CheckIfTokenAlreadyExists(string token)
    {
        return await _context.ExpiredTokens.AnyAsync(x => x.Key == token);
    }
}