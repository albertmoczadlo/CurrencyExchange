using VistulaExchange.Database.Domain;

namespace VistulaExchange.Database.Interface
{
    public interface IUserWalletRepository
    {
        Task<Wallet> GetWalletAsync(string userId);
        Task DepositAsync(string userId, string currencyCode, decimal depositAmount, string description);
        Task WithdrawalAsync(string userId, string currencyCode, decimal withdrawalAmount, string description);
    }
}
