using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Web.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace JediApp.Database.Repositories
{
    public class TransactionHistoryRepositoryDB : ITransactionHistoryRepository
    {
        private readonly JediAppDbContext _jediAppDb;

        public TransactionHistoryRepositoryDB(JediAppDbContext jediAppDb)
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
