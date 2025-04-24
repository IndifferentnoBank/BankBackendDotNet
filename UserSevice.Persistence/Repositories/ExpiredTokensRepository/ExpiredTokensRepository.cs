using Common.GenericRepository;
using Microsoft.EntityFrameworkCore;
using UserService.Contracts.Repositories;
using UserService.Domain.Entities;
using UserSevice.Persistence;

namespace UserService.Persistence.Repositories.ExpiredTokensRepository;

public class ExpiredTokensRepository : GenericRepository<ExpiredToken>, IExpiredTokensRepository
{
    private readonly UserServiceDbContext _context;
    public ExpiredTokensRepository(UserServiceDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<bool> CheckIfTokenAlreadyExists(string token)
    {
        return await _context.ExpiredTokens.AnyAsync(x => x.Key == token);
    }
}