using JediApp.Database.Domain;

namespace JediApp.Services.Services
{
    public interface ITransactionHistoryService
    {
        public bool AddTransaction(TransactionHistory transactionHistory);
        //public List<TransactionHistory> GetUserHistoryByUserId(Guid userId);
        public List<TransactionHistory> GetUserHistoryByUserId(string userId);

        public List<TransactionHistory> GetAllUsersHistories();

    }
}
