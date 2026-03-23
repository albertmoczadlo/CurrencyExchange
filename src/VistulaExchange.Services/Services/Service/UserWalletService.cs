using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Interfaces;

namespace VistulaExchange.Services.Services.Service
{
    public class UserWalletService : IUserWalletService
    {
        private readonly IUserWalletRepository _userWalletRepository;

        public UserWalletService(IUserWalletRepository userWalletRepository)
        {
            _userWalletRepository = userWalletRepository;
        }

        public Task<Wallet> GetWalletAsync(string userId)
        {
            return _userWalletRepository.GetWalletAsync(userId);
        }

        public async Task<WalletPosition> GetCurrencyBalanceByIdAsync(string userId, Guid currencyId)
        {
            var wallet = await _userWalletRepository.GetWalletAsync(userId);

            var currencyAmount = wallet.WalletPositions?
                .FirstOrDefault(a => a.Currency.Id == currencyId);

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

        public async Task<WalletPosition> GetCurrencyBalanceByCodeAsync(string userId, string currencyCode)
        {
            var wallet = await _userWalletRepository.GetWalletAsync(userId);

            return wallet.WalletPositions?
                .FirstOrDefault(a => string.Equals(a.Currency.ShortName, currencyCode, StringComparison.OrdinalIgnoreCase))
                ?? new WalletPosition();
        }

        public Task WithdrawalAsync(string userId, string currencyCode, decimal withdrawalAmount, string description)
        {
            return _userWalletRepository.WithdrawalAsync(userId, currencyCode, withdrawalAmount, description);
        }

        public Task DepositAsync(string userId, string currencyCode, decimal depositAmount, string description)
        {
            return _userWalletRepository.DepositAsync(userId, currencyCode, depositAmount, description);
        }
    }
}
