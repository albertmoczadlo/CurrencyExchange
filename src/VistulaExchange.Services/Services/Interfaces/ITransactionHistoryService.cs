using VistulaExchange.Database.Domain;

namespace VistulaExchange.Services.Services.Interfaces
{
    public interface ITransactionHistoryService
    {
        Task<bool> AddTransactionAsync(TransactionHistory transactionHistory);
        Task<List<TransactionHistory>> GetUserHistoryByUserIdAsync(string userId);
        Task<List<TransactionHistory>> GetAllUsersHistoriesAsync();
    }
}
