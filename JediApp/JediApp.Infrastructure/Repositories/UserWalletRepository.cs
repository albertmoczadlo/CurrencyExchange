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

            // Update or add wallet currency
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

        //        var userWalletIdFromFile = columns[0];
        //        if (userWalletIdFromFile.Equals(userWalletId, StringComparison.InvariantCultureIgnoreCase))
        //            userWallet.WalletStatus.Add(new WalletPosition(columns[0]) { Currency = new Currency() { ShortName = columns[2] }, CurrencyAmount = decimal.Parse(columns[3]), UserName = columns[1] });
        //    }

        //    return userWallet;
        //}

        //public void Deposit(Guid walletId, string userName, string currencyCode, decimal depositAmount)
        //{
        //    RecalculateUserWalletRepository(walletId, userName, currencyCode, depositAmount, true);
        //}

        //public void Withdrawal(Guid walletId, string userName, string currencyCode, decimal withdrawalAmount)
        //{
        //    RecalculateUserWalletRepository(walletId, userName, currencyCode, withdrawalAmount, false);
        //}

        //public List<Wallet> GetAllWallets()
        //{
        //    var wallets = new List<Wallet>();

        //    if (!File.Exists(_fileNameWallet))
        //        return wallets;

        //    var walletPositionsFromFile = File.ReadAllLines(_fileNameWallet);
        //    var walletPositions = new List<WalletPosition>();

        //    foreach (var line in walletPositionsFromFile)
        //    {
        //        string[] columns = line.Split(";");
        //        walletPositions.Add(new WalletPosition(columns[0]) { Currency = new Currency() { ShortName = columns[2] }, CurrencyAmount = decimal.Parse(columns[3]), UserName = columns[1] });
        //    }

        //    var grouppedWalPos = walletPositions.GroupBy(a => a.WalletId);
        //    foreach (var group in grouppedWalPos)
        //    {
        //        wallets.Add(new Wallet(group.Key.ToString()) { WalletStatus = group.ToList() });
        //    }

        //    return wallets;
        //}

        //private void RecalculateUserWalletRepository(Guid walletId, string userName, string currencyCode, decimal amount, bool isDeposit)
        //{
        //    var allWalletsExceptUser = GetAllWallets().Where(a => a.Id != walletId);
        //    var userWallet = GetWallet(walletId);

        //    if (File.Exists(_fileNameWallet))
        //        File.Delete(_fileNameWallet);

        //    var userCurrency = userWallet.WalletStatus.Where(w => w.Currency.ShortName.Equals(currencyCode)).SingleOrDefault();
        //    if (userCurrency != null)
        //    {
        //        if (isDeposit)
        //        {
        //            userCurrency.CurrencyAmount += amount;
        //        }
        //        else
        //        {
        //            userCurrency.CurrencyAmount -= amount;
        //        }
        //    }
        //    else
        //    {
        //        userWallet.WalletStatus.Add(new WalletPosition
        //        {
        //            Currency = new Currency
        //            {
        //                ShortName = currencyCode
        //            },
        //            CurrencyAmount = amount,
        //        });
        //    }

        //    // Add all users positions
        //    using (StreamWriter file = new StreamWriter(_fileNameWallet, true))
        //    {
        //        foreach (var previousPoistions in userWallet.WalletStatus)
        //        {
        //            file.WriteLine($"{walletId};{userName};{previousPoistions.Currency.ShortName};{previousPoistions.CurrencyAmount}");
        //        }
        //    }

        //    // Add all other deposits from memory
        //    foreach (var wallet in allWalletsExceptUser)
        //    {
        //        foreach (var walPos in wallet.WalletStatus)
        //        {
        //            using (StreamWriter file = new StreamWriter(_fileNameWallet, true))
        //            {
        //                file.WriteLine($"{wallet.Id};{walPos.UserName};{walPos.Currency.ShortName};{walPos.CurrencyAmount}");
        //            }
        //        }
        //    }

    }
}
