using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Interfaces;

namespace VistulaExchange.Services.Services.Service
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly ITransactionHistoryRepository _transactionHistoryRepository;

        public TransactionHistoryService(ITransactionHistoryRepository transactionHistoryRepository)
        {
            _transactionHistoryRepository = transactionHistoryRepository;
        }

        public Task<bool> AddTransactionAsync(TransactionHistory transactionHistory)
        {
            return _transactionHistoryRepository.AddTransactionAsync(transactionHistory);
        }

        public Task<List<TransactionHistory>> GetUserHistoryByUserIdAsync(string userId)
        {
            return _transactionHistoryRepository.GetUserHistoryByUserIdAsync(userId);
        }

        public Task<List<TransactionHistory>> GetAllUsersHistoriesAsync()
        {
            return _transactionHistoryRepository.GetAllUsersHistoriesAsync();
        }
    }
}
