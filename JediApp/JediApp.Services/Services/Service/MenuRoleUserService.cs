using JediApp.Database.Domain;
using JediApp.Database.Interface;

using JediApp.Services.Helpers;
using JediApp.Services.Services.Interfaces;

namespace JediApp.Services.Services.Service
{
    public class MenuRoleUserService
    {
        private readonly IUserService _userService;
        private readonly IUserWalletRepository _userWalletRepository;
        private readonly IExchangeOfficeBoardService _exchangeOfficeBoardSevice;
        private readonly ITransactionHistoryService _transactionHistoryService;

        public MenuRoleUserService(IUserService userService, IUserWalletRepository userWalletRepository, IExchangeOfficeBoardService exchangeOfficeBoardSevice, ITransactionHistoryService transactionHistoryService)
        {
            _userService = userService;
            _userWalletRepository = userWalletRepository;
            _exchangeOfficeBoardSevice = exchangeOfficeBoardSevice;
            _transactionHistoryService = transactionHistoryService;
        }

        //public void RegisterUser()
        //{
        //    Console.Clear();

        //    var user = new User();
        //    user.Role = UserRole.User;  //jako enum albo tabela w bazie

        //    Console.WriteLine("Enter new login");
        //    user.Login = MenuOptionsHelper.CheckString(Console.ReadLine());

        //    Console.WriteLine("Enter password");
        //    user.Password = MenuOptionsHelper.CheckString(Console.ReadLine());

        //    var registeredUser = _userService.AddUser(user);
        //    if (registeredUser != null)
        //    {
        //        Console.WriteLine($"Login {user.Login} has been created.");
        //    }
        //    else
        //    {
        //        if (!_userService.IfLoginIsUnique(user))
        //        {
        //            Console.WriteLine($"Login {user.Login} has already been used. Try another one...");
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Login {user.Login} has NOT been created.");
        //        }
        //    }
        //}

        //public Wallet GetWallet(User user)
        //{
        //    Console.Clear();

        //    var userWallet = _userWalletRepository.GetWallet(user.Wallet.Id);

        //    Console.WriteLine($"User: {user.Login} : your current balance:");
        //    foreach (var item in userWallet.WalletStatus)
        //    {
        //        Console.WriteLine($"{item.Currency.ShortName}:{item.CurrencyAmount}");
        //    }

        //    return userWallet;
        //}

        //public void Deposit(User user)
        //{
        //    Console.Clear();

        //    var availableCurrencies = _exchangeOfficeBoardSevice.GetAllCurrencies();
        //    Console.WriteLine("Select available currency:");

        //    for (var i = 0; i < availableCurrencies.Count; i++)
        //    {
        //        Console.WriteLine($"{i + 1}. Currency: {availableCurrencies[i].Name}, Code: {availableCurrencies[i].ShortName}");
        //    }
        //    var selectedCurrencyIndex = MenuOptionsHelper.GetUserSelectionAndValidate(1, availableCurrencies.Count);
        //    var selectedCurrencyName = availableCurrencies[selectedCurrencyIndex - 1].ShortName;

        //    Console.WriteLine($"You selected: {selectedCurrencyName}");
        //    Console.WriteLine("Enter amout of money for deposit:");
        //    var deposit = MenuOptionsHelper.CheckDecimal(Console.ReadLine());


        //    //Console.WriteLine($"Before deposit {user.Wallet.Id}, {user.Login}");
        //    //_userWalletRepository.Deposit(user.Wallet.Id, user.Login, selectedCurrencyName, deposit);

        //    Console.WriteLine("Deposit successfull!\n");

        //    PrintWalletCurrencySaldo(user, selectedCurrencyName);

        //    _transactionHistoryService.AddTransaction(new TransactionHistory { Id = Guid.NewGuid(), UserId = user.Id, UserLogin = user.Login, CurrencyName = selectedCurrencyName, Amount = deposit, DateOfTransaction = DateTime.Now, Description = "Deposit" });
        //}

        //public void Exchange(User user)
        //{
        //    Console.Clear();

        //    var userWallet = _userWalletRepository.GetWallet(user.Wallet.Id);
        //    if (!userWallet.WalletStatus.Any())
        //    {
        //        Console.WriteLine("You don't have any currency in your deposit");
        //        return;
        //    }

        //    Console.WriteLine("Select available currency from you wallet for exchange:");
        //    for (var i = 0; i < userWallet.WalletStatus.Count; i++)
        //    {
        //        Console.WriteLine($"{i + 1}. Currency: {userWallet.WalletStatus[i].Currency.ShortName}, current saldo: {userWallet.WalletStatus[i].CurrencyAmount}");
        //    }
        //    var selectedCurrencyIndex = MenuOptionsHelper.GetUserSelectionAndValidate(1, userWallet.WalletStatus.Count);
        //    var selectedCurrencyName = userWallet.WalletStatus[selectedCurrencyIndex - 1].Currency.ShortName;

        //    Console.WriteLine($"You selected: {selectedCurrencyName}");

        //    decimal amountInput;
        //    Currency exchangeToCurrency = null;

        //    if (selectedCurrencyName == "PLN")
        //    {
        //        Console.WriteLine("Select currency to exchange to");
        //        var availableCurrencies = _exchangeOfficeBoardSevice.GetAllCurrencies();
        //        Console.WriteLine("Select from available currency:");

        //        for (var i = 0; i < availableCurrencies.Count; i++)
        //        {
        //            Console.WriteLine($"{i + 1}. Currency: {availableCurrencies[i].Name}, Code: {availableCurrencies[i].ShortName}");
        //        }

        //        var exchangeToCurrecnyIndex = MenuOptionsHelper.GetUserSelectionAndValidate(1, availableCurrencies.Count);
        //        exchangeToCurrency = availableCurrencies[exchangeToCurrecnyIndex - 1];
        //    }

        //    while (true)
        //    {
        //        Console.WriteLine("Enter amout of money for exchange:");

        //        amountInput = MenuOptionsHelper.CheckDecimal(Console.ReadLine());

        //        var currentSaldo = GetCurrencySaldo(user, selectedCurrencyName);
        //        if (amountInput > currentSaldo)
        //        {
        //            Console.WriteLine("You do not have available funds on the account.");
        //            Console.WriteLine("Please enter correct amount.");
        //            continue;
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }

        //    if (exchangeToCurrency == null)
        //    {
        //        var currency = _exchangeOfficeBoardSevice.GetAllCurrencies().Single(x => x.ShortName == selectedCurrencyName);
        //        decimal plnAmount = amountInput * currency.SellAt;

        //        Console.WriteLine($"Withdrowing {amountInput} of {selectedCurrencyName}");
        //        _userWalletRepository.Withdrawal(user.Wallet.Id, user.Login, selectedCurrencyName, amountInput);

        //        Console.WriteLine($"Depositing {plnAmount} on PLN");
        //        _userWalletRepository.Deposit(user.Wallet.Id, user.Login, "PLN", plnAmount);
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Withdrowing {amountInput} of PLN");
        //        _userWalletRepository.Withdrawal(user.Wallet.Id, user.Login, "PLN", amountInput);

        //        decimal newCurrencyAmount = amountInput / exchangeToCurrency.BuyAt;
        //        Console.WriteLine($"Depositing {newCurrencyAmount} of {exchangeToCurrency.ShortName}");
        //        _userWalletRepository.Deposit(user.Wallet.Id, user.Login, exchangeToCurrency.ShortName, newCurrencyAmount);
        //    }

        //}

        //public void Withdrawal(User user)
        //{
        //    Console.Clear();

        //    var userWallet = _userWalletRepository.GetWallet(user.Wallet.Id);
        //    if (!userWallet.WalletStatus.Any())
        //    {
        //        Console.WriteLine("You don't have any currency in your deposit");
        //        return;
        //    }

        //    Console.WriteLine("Select available currency from you wallet:");
        //    for (var i = 0; i < userWallet.WalletStatus.Count; i++)
        //    {
        //        Console.WriteLine($"{i + 1}. Currency: {userWallet.WalletStatus[i].Currency.ShortName}, current saldo: {userWallet.WalletStatus[i].CurrencyAmount}");
        //    }
        //    var selectedCurrencyIndex = MenuOptionsHelper.GetUserSelectionAndValidate(1, userWallet.WalletStatus.Count);
        //    var selectedCurrencyName = userWallet.WalletStatus[selectedCurrencyIndex - 1].Currency.ShortName;

        //    Console.WriteLine($"You selected: {selectedCurrencyName}");

        //    decimal withdrawal;

        //    while (true)
        //    {
        //        Console.WriteLine("Enter amout of money for withdrawal:");

        //        withdrawal = MenuOptionsHelper.CheckDecimal(Console.ReadLine());

        //        var currentSaldo = GetCurrencySaldo(user, selectedCurrencyName);
        //        if (withdrawal > currentSaldo)
        //        {
        //            Console.WriteLine("You do not have available funds on the account.");
        //            Console.WriteLine("Please enter correct amount.");
        //            continue;
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }

        //    _userWalletRepository.Withdrawal(user.Wallet.Id, user.Login, selectedCurrencyName, withdrawal);

        //    Console.WriteLine("Withdrawal successfull!\n");

        //    PrintWalletCurrencySaldo(user, selectedCurrencyName);

        //    _transactionHistoryService.AddTransaction(new TransactionHistory { Id = Guid.NewGuid(), UserId = user.Id, UserLogin = user.Login, CurrencyName = selectedCurrencyName, Amount = withdrawal, DateOfTransaction = DateTime.Now, Description = "Withdrawal" });
        //}

        //private decimal GetCurrencySaldo(User user, string selectedCurrencyName)
        //{
        //    var userWallet = _userWalletRepository.GetWallet(user.Wallet.Id);
        //    var currentSaldo = userWallet.WalletStatus.Where(a => a.Currency.ShortName.Equals(selectedCurrencyName, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();

        //    return currentSaldo.CurrencyAmount;
        //}

        //private void PrintWalletCurrencySaldo(User user, string selectedCurrencyName)
        //{
        //    var currentSaldo = GetCurrencySaldo(user, selectedCurrencyName);

        //    Console.WriteLine($"Your current saldo of '{selectedCurrencyName}' is {currentSaldo}");
        //}

        //public void GetUserHistory(User user)
        //{
        //    var transactionHistory = _transactionHistoryService.GetUserHistoryByUserId(user.Id);

        //    Console.Clear();
        //    Console.WriteLine($"Transactions history for Login: {user.Login}");
        //    transactionHistory.ForEach(x => Console.WriteLine($"{x.DateOfTransaction} {x.Description} {x.CurrencyName} {x.Amount}"));


        //}
    }
}
