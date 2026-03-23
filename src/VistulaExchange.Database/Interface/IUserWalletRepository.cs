using VistulaExchange.Database.Domain;

namespace VistulaExchange.Database.Interface
{
    public interface IUserWalletRepository
    {
        public Wallet GetWallet(string userId);
        public void Deposit(string userId, string currencyCode, decimal depositAmount, string description);
        public void Withdrawal(string userId, string currencyCode, decimal withdrawalAmount, string description);
    }
}
