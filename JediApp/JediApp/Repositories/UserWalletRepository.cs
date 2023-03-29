using JediApp.Database.Domain;

namespace JediApp.Database.Repositories
{
    public class UserWalletRepository  /*:IUserWalletRepository*/
    {
        private readonly string _fileNameWallet = "..//..//..//..//Userwallets.csv";

        public void RegisterWalletToUser(Guid walletId, string userLogin, string newCurrencyCode, decimal newCurrencyAmount)
        {
            using (StreamWriter file = new StreamWriter(_fileNameWallet, true))
            {
                file.WriteLine($"{walletId};{userLogin};{newCurrencyCode};{newCurrencyAmount}");
            }
        }

        //public Wallet GetWallet(Guid walletId)
        //{
        //    var userWallet = new Wallet(walletId.ToString());
        //    var userWalletId = walletId.ToString();

        //    if (!File.Exists(_fileNameWallet))
        //        return userWallet;

        //    var walletPositionsFromFile = File.ReadAllLines(_fileNameWallet);

        //    foreach (var line in walletPositionsFromFile)
        //    {
        //        string[] columns = line.Split(";");

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
