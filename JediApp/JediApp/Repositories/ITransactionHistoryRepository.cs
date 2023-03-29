using JediApp.Database.Domain;

namespace JediApp.Database.Repositories
{
    public interface ITransactionHistoryRepository
    {
        public bool AddTransaction(TransactionHistory transactionHistory);
        public List<TransactionHistory> GetUserHistoryByUserId(Guid userId);
        public List<TransactionHistory> GetAllUsersHistories();

    }
}
