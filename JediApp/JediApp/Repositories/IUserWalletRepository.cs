using JediApp.Database.Domain;

namespace JediApp.Database.Repositories
{
    public interface IUserWalletRepository
    {
        public Wallet GetWallet(Guid walletId);
        public void RegisterWalletToUser(Guid walletId, string userLogin, string newCurrencyCode, decimal newCurrencyAmount);
        public void Deposit(Guid walletId, string userLogin, string currencyCode, decimal depositAmount);
        public void Withdrawal(Guid walletId, string userLogin, string currencyCode, decimal withdrawalAmount);
    }
}
