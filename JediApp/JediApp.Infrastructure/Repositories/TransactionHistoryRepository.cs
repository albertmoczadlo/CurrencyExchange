using JediApp.Database.Domain;
using JediApp.Database.Interface;

namespace JediApp.Database.Repositories
{
    public class TransactionHistoryRepository : ITransactionHistoryRepository
    {
        private readonly string fileName = "..//..//..//..//TransactionHistory.csv"; 
        public bool AddTransaction(TransactionHistory transactionHistory)
        {
            using (StreamWriter file = new StreamWriter(fileName, true))
            {
                file.WriteLine($"{transactionHistory.Id};{transactionHistory.UserId};{transactionHistory.CurrencyName};{transactionHistory.Amount};{transactionHistory.DateOfTransaction};{transactionHistory.Description}");
            }

            return true;
        }

        public List<TransactionHistory> GetAllUsersHistories()
        {
            List<TransactionHistory> transactionHistory = new List<TransactionHistory>();

            if (!File.Exists(fileName))
                return new List<TransactionHistory>();

            var transactionHistoryFromFile = File.ReadAllLines(fileName);
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
        public List<TransactionHistory> GetUserHistoryByUserId(string userId)
        {
            List<TransactionHistory> transactionHistory = GetAllUsersHistories();

            return transactionHistory.Where(x => String.Equals(x.UserId, userId)).ToList(); 

        }
    }
}
