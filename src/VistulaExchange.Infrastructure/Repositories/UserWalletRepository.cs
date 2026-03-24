using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace VistulaExchange.Infrastructure.Repositories
{
    public class UserWalletRepository : IUserWalletRepository
    {
        private readonly VistulaExchangeDbContext _dbContext;

        public UserWalletRepository(VistulaExchangeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DepositAsync(string userId, string currencyCode, decimal depositAmount, string description)
        {
            var userWallet = await GetWalletAsync(userId);

            WalletPosition walletCurrency;
            if (userWallet.WalletPositions is not null && userWallet.WalletPositions.Any())
            {
                walletCurrency = userWallet.WalletPositions
                    .FirstOrDefault(cur => string.Equals(cur.Currency.ShortName, currencyCode, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                userWallet.WalletPositions = new List<WalletPosition>();
                walletCurrency = null;
            }

            if (walletCurrency != null)
            {
                walletCurrency.CurrencyAmount += depositAmount;
                _dbContext.Update(walletCurrency);
            }
            else
            {
                var currency = await _dbContext.Currencys
                    .SingleAsync(c => string.Equals(c.ShortName, currencyCode, StringComparison.OrdinalIgnoreCase));

                userWallet.WalletPositions.Add(new WalletPosition
                {
                    CurrencyAmount = depositAmount,
                    CurrencyId = currency.Id
                });
            }

            var transactionHistory = new TransactionHistory
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CurrencyName = currencyCode,
                Amount = depositAmount,
                DateOfTransaction = DateTime.Now,
                Description = description
            };

            await _dbContext.AddAsync(transactionHistory);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Wallet> GetWalletAsync(string userId)
        {
            var userWalletExists = await _dbContext.Wallets.AnyAsync(a => a.UserId == userId);

            if (!userWalletExists)
            {
                await _dbContext.Wallets.AddAsync(new Wallet { UserId = userId });
                await _dbContext.SaveChangesAsync();
            }

            return await _dbContext.Wallets
                .Include(a => a.WalletPositions)
                .ThenInclude(a => a.Currency)
                .SingleAsync(a => a.UserId == userId);
        }

        public async Task WithdrawalAsync(string userId, string currencyCode, decimal withdrawalAmount, string description)
        {
            var userWallet = await GetWalletAsync(userId);

            WalletPosition walletCurrency;
            if (userWallet.WalletPositions is not null && userWallet.WalletPositions.Any())
            {
                walletCurrency = userWallet.WalletPositions
                    .FirstOrDefault(cur => string.Equals(cur.Currency.ShortName, currencyCode, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                return;
            }

            if (walletCurrency is null)
            {
                return;
            }

            walletCurrency.CurrencyAmount -= withdrawalAmount;
            _dbContext.Update(walletCurrency);

            var transactionHistory = new TransactionHistory
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CurrencyName = currencyCode,
                Amount = withdrawalAmount,
                DateOfTransaction = DateTime.Now,
                Description = description
            };

            await _dbContext.AddAsync(transactionHistory);
            await _dbContext.SaveChangesAsync();
        }
    }
}
