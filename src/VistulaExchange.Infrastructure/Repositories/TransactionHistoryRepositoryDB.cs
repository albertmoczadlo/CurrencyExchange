using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace VistulaExchange.Infrastructure.Repositories
{
    public class TransactionHistoryRepositoryDB : ITransactionHistoryRepository
    {
        private readonly VistulaExchangeDbContext _dbContext;

        public TransactionHistoryRepositoryDB(VistulaExchangeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddTransactionAsync(TransactionHistory transactionHistory)
        {
            await _dbContext.AddAsync(transactionHistory);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public Task<List<TransactionHistory>> GetAllUsersHistoriesAsync()
        {
            return _dbContext.TransactionHistory
                .Include(t => t.User)
                .OrderBy(c => c.DateOfTransaction)
                .ToListAsync();
        }

        public Task<List<TransactionHistory>> GetUserHistoryByUserIdAsync(string userId)
        {
            return _dbContext.TransactionHistory
                .Where(x => x.UserId == userId)
                .Include(t => t.User)
                .OrderBy(c => c.DateOfTransaction)
                .ToListAsync();
        }
    }
}
