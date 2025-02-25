using System.Transactions;
using Common.GenericRepository;
using Microsoft.EntityFrameworkCore;
using Transaction = CoreService.Domain.Entities.Transaction;

namespace CoreService.Persistence.Repositories.TransactionsRepository;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    private readonly CoreServiceDbContext _dbContext;
    
    public TransactionRepository(CoreServiceDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task<List<Transaction>> GetAllByAccountIdAsync(Guid accountId)
    {
       return await _dbContext.Transactions.Where(x=>x.BankAccountId == accountId).ToListAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid transactionId)
    {
        return await _dbContext.Transactions.FirstOrDefaultAsync(x=>x.Id == transactionId);
    }
}