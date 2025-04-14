using Common.GenericRepository;
using CoreService.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Transaction = CoreService.Domain.Entities.Transaction;
using TransactionStatus = CoreService.Domain.Enums.TransactionStatus;

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

    public Task<List<Transaction>> GetByStatusAsync(TransactionStatus status)
    {
        return  Task.FromResult(_dbContext.Transactions.Where(x=>x.Status == status).ToList());
    }
}