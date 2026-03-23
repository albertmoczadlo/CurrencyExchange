using VistulaExchange.Database.Domain;

namespace VistulaExchange.Database.Interface
{
    public interface ITransactionHistoryRepository
    {
        Task<bool> AddTransactionAsync(TransactionHistory transactionHistory);
        Task<List<TransactionHistory>> GetUserHistoryByUserIdAsync(string userId);
        Task<List<TransactionHistory>> GetAllUsersHistoriesAsync();
    }
}
