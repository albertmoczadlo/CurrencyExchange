using JediApp.Database.Domain;
using JediApp.Database.Interface;
using System.Security.Claims;

namespace JediApp.Services.Services
{
    public class UserWalletService : IUserWalletService
    {
        private readonly IUserWalletRepository _userWalletRepository;

        public UserWalletService(IUserWalletRepository userWalletRepository)
        {
            _userWalletRepository = userWalletRepository;
        }

        public WalletPosition GetCurrencyBalanceById(string userId, Guid currencyId)
        {
            var wallet = _userWalletRepository.GetWallet(userId);

            var currencyAmount = wallet.WalletPositions.Where(a => a.Currency.Id == currencyId).FirstOrDefault();

            if (currencyAmount is null)
            {
                currencyAmount = new WalletPosition
                {
                    CurrencyId = currencyId,
                    CurrencyAmount = 0
                };
            }

            return currencyAmount;
        }

        public void Withdrawal(string userId, string currencyCode, decimal withdrawalAmount, string description)
        {
            _userWalletRepository.Withdrawal(userId, currencyCode, withdrawalAmount, description);
        }

        public void Deposit(string userId, string currencyCode, decimal depositAmount, string description)
        {
            _userWalletRepository.Deposit(userId, currencyCode, depositAmount, description);
        }

      




    }
}
