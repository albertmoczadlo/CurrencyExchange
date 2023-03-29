using JediApp.Database.Domain;

namespace JediApp.Services.Services
{
    public interface IUserWalletService
    {
        WalletPosition GetCurrencyBalanceById(string userId, Guid currencyId);
        void Withdrawal(string userId, string currencyCode, decimal withdrawalAmount, string description);
        void Deposit(string userId, string currencyCode, decimal depositAmount, string description);

    }
}
