using VistulaExchange.Database.Domain;

namespace VistulaExchange.Database.Interface
{
    public interface ITransactionHistoryRepository
    {
        public bool AddTransaction(TransactionHistory transactionHistory);
        public List<TransactionHistory> GetUserHistoryByUserId(string userId);
        public List<TransactionHistory> GetAllUsersHistories();

    }
}
