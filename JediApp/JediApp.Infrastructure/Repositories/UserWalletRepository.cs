using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Web.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace JediApp.Database.Repositories
{
    public class UserWalletRepository : IUserWalletRepository
    {
        private readonly JediAppDbContext _jediAppDb;

        public UserWalletRepository(JediAppDbContext jediAppDb)
        {
            _jediAppDb = jediAppDb;
        }

        public void Deposit(string userId, string currencyCode, decimal depositAmount, string description)
        {
            var userWallet = GetWallet(userId);

            WalletPosition walletCurrency;
            if (userWallet.WalletPositions != null && userWallet.WalletPositions.Any())
            {
                walletCurrency = userWallet.WalletPositions.FirstOrDefault(cur => cur.Currency.ShortName == currencyCode);
            }
            else
            {
                userWallet.WalletPositions = new List<WalletPosition>();
                walletCurrency = null;
            }

            if (walletCurrency != null)
            {
                walletCurrency.CurrencyAmount = walletCurrency.CurrencyAmount + depositAmount;
                _jediAppDb.Update(walletCurrency);
            }
            else
            {
                var currency = _jediAppDb.Currencys.Single(c => c.ShortName == currencyCode);


                userWallet.WalletPositions.Add(new Database.Domain.WalletPosition()
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

            if (description != "Sell")
            {
                _jediAppDb.Add(transactionHistory);
            }

            _jediAppDb.SaveChanges();
        }

        public Wallet GetWallet(string userId)
        {
            var userWalletExists = _jediAppDb.Wallets.Any(a => a.UserId == userId);

            if (!userWalletExists)
            {
                _jediAppDb.Wallets.Add(new Database.Domain.Wallet() { UserId = userId });
                _jediAppDb.SaveChanges();
            }

            return _jediAppDb.Wallets.Include("WalletPositions.Currency").SingleOrDefault(a => a.UserId == userId);
        }

        public void Withdrawal(string userId, string currencyCode, decimal withdrawalAmount, string description)
        {
            var userWallet = GetWallet(userId);

            WalletPosition walletCurrency;
            if (userWallet.WalletPositions != null && userWallet.WalletPositions.Any())
            {
                walletCurrency = userWallet.WalletPositions.FirstOrDefault(cur => cur.Currency.ShortName == currencyCode);
            }
            else
            {
                return;
            }

            walletCurrency.CurrencyAmount = walletCurrency.CurrencyAmount - withdrawalAmount;
            _jediAppDb.Update(walletCurrency);

            var transactionHistory = new TransactionHistory
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CurrencyName = currencyCode,
                Amount = withdrawalAmount,
                DateOfTransaction = DateTime.Now,
                Description = description
            };

            if (description != "Buy")
            {
                _jediAppDb.Add(transactionHistory);
            }

            _jediAppDb.SaveChanges();
        }
    }
}
