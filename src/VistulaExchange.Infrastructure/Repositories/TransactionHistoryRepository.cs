using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;

namespace VistulaExchange.Infrastructure.Repositories
{
    public class TransactionHistoryRepository : ITransactionHistoryRepository
    {
        private readonly string fileName = "..//..//..//..//TransactionHistory.csv";

        public async Task<bool> AddTransactionAsync(TransactionHistory transactionHistory)
        {
            await using (StreamWriter file = new StreamWriter(fileName, true))
            {
                await file.WriteLineAsync($"{transactionHistory.Id};{transactionHistory.UserId};{transactionHistory.CurrencyName};{transactionHistory.Amount};{transactionHistory.DateOfTransaction};{transactionHistory.Description}");
            }

            return true;
        }

        public async Task<List<TransactionHistory>> GetAllUsersHistoriesAsync()
        {
            List<TransactionHistory> transactionHistory = new List<TransactionHistory>();

            if (!File.Exists(fileName))
                return new List<TransactionHistory>();

            var transactionHistoryFromFile = await File.ReadAllLinesAsync(fileName);
            foreach (var line in transactionHistoryFromFile)
            {
                var columns = line.Split(';');
                Guid.TryParse(columns[0], out var transactionId);
                decimal.TryParse(columns[4], out var amount);
                DateTime.TryParse(columns[5], out var dateOfTransaction);
                transactionHistory.Add(new TransactionHistory { Id = transactionId, UserId = columns[1], CurrencyName = columns[3], Amount = amount, DateOfTransaction = dateOfTransaction, Description = columns[6] });
            }
            return transactionHistory;
        }

        public async Task<List<TransactionHistory>> GetUserHistoryByUserIdAsync(string userId)
        {
            List<TransactionHistory> transactionHistory = await GetAllUsersHistoriesAsync();

            return transactionHistory.Where(x => String.Equals(x.UserId, userId)).ToList();
        }
    }
}
