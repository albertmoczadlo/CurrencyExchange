using VistulaExchange.Database.Domain;

namespace VistulaExchange.Services.Services.Interfaces
{
    public interface IUserWalletService
    {
        Task<Wallet> GetWalletAsync(string userId);
        Task<WalletPosition> GetCurrencyBalanceByIdAsync(string userId, Guid currencyId);
        Task<WalletPosition> GetCurrencyBalanceByCodeAsync(string userId, string currencyCode);
        Task WithdrawalAsync(string userId, string currencyCode, decimal withdrawalAmount, string description);
        Task DepositAsync(string userId, string currencyCode, decimal depositAmount, string description);
    }
}
