using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Web.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace VistulaExchange.Database.Repositories
{
    public class TransactionHistoryRepositoryDB : ITransactionHistoryRepository
    {
        private readonly VistulaExchangeDbContext _jediAppDb;

        public TransactionHistoryRepositoryDB(VistulaExchangeDbContext jediAppDb)
        {
            _jediAppDb = jediAppDb;
        }
        public bool AddTransaction(TransactionHistory transactionHistory)
        {
            _jediAppDb.Add(transactionHistory);
            _jediAppDb.SaveChanges();

            return true;
        }

        public List<TransactionHistory> GetAllUsersHistories()
        {
            return _jediAppDb.TransactionHistory.Include(t => t.User).OrderBy(c => c.DateOfTransaction).ToList();
        }

        public List<TransactionHistory> GetUserHistoryByUserId(string userId)
        {

            return _jediAppDb.TransactionHistory.Where(x => x.UserId == userId).Include(t => t.User).OrderBy(c => c.DateOfTransaction).ToList();
        }
    }
}
