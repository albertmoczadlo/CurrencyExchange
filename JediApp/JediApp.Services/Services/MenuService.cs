using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Services.Helpers;

namespace JediApp.Services.Services
{
    public class MenuService
    {
        private readonly IUserService _userService;
        private readonly MenuRoleUserService _menuUserActions;
        private readonly MenuRoleAdminService _menuAdminActions;
        private readonly IExchangeOfficeBoardService _exchangeOfficeBoardSevice;
        private readonly INbpJsonService _nbpJsonService;
        private readonly ITransactionHistoryService _transactionHistoryService;
        private User CurrentUser;



        public MenuService(IUserService userService, IUserWalletRepository userWalletRepository, IExchangeOfficeBoardService exchangeOfficeBoardSevice, IAvailableMoneyOnStockRepository availableMoneyOnStock, INbpJsonService nbpJsonService, ITransactionHistoryService transactionHistoryService)

        {
            _userService = userService;
            _nbpJsonService = nbpJsonService;
            _exchangeOfficeBoardSevice = exchangeOfficeBoardSevice;
            _transactionHistoryService = transactionHistoryService;
            _menuUserActions = new MenuRoleUserService(_userService, userWalletRepository, _exchangeOfficeBoardSevice, _transactionHistoryService);
            _menuAdminActions = new MenuRoleAdminService(_userService, _exchangeOfficeBoardSevice, availableMoneyOnStock, _nbpJsonService, _transactionHistoryService);
            
        }

        //public void WelcomeMenu()
        //{
        //    Console.Clear();
        //    Console.WriteLine("**** JediApp ****");
        //    Console.WriteLine("\nWelcome! Please select option:");
        //    Console.WriteLine("1. Login User");
        //    Console.WriteLine("2. Register User");
        //    Console.WriteLine("3. Exchange Office Board");
        //    Console.WriteLine("4. NBP Today Rates");
        //    Console.WriteLine("5. Exit");
        //    Console.WriteLine("You choose: ");

        //    int selectedOption = MenuOptionsHelper.GetUserSelectionAndValidate(1, 5);
        //    Console.Clear();

        //    switch (selectedOption)
        //    {
        //        case 1:

        //            List<User> users = new List<User>();

        //            while (true)
        //            {
        //                Console.WriteLine("Enter your login.");
        //                string login = Helpers.MenuOptionsHelper.CheckString(Console.ReadLine());

        //                Console.WriteLine("Enter password");
        //                string password = Helpers.MenuOptionsHelper.CheckString(Console.ReadLine());

        //                CurrentUser = new UserRepository().GetLoginPassword(login, password);

        //                if (CurrentUser != null)
        //                {
        //                    Console.WriteLine("Password provided is correct");
        //                    Console.WriteLine($"Welcome {CurrentUser.Login}");
        //                    break;
        //                }
        //                else
        //                {
        //                    Console.WriteLine("User password / login error.");
        //                }
        //            }

        //            if (CurrentUser.Role == UserRole.Admin)
        //            {
        //                AdminMenu();
        //            }
        //            else
        //            {
        //                UserMenu();
        //            }
        //            break;

        //        case 2:
        //            _menuUserActions.RegisterUser();
        //            WelcomeMenu();
        //            break;
        //        case 3:
        //            PrintExchangeOfficeBoard();
        //            ExchangeOfficeBoardMenu("Exchange Office Board");
        //            WelcomeMenu();
        //            break;
        //        case 4:
        //            PrintNBPRates();
        //            ExchangeOfficeBoardMenu("NBP Today Rates");
        //            WelcomeMenu();
        //            break;
        //        case 5:
        //            Console.WriteLine("Exit from app, bye, bye!!!");
        //            Environment.Exit(0);
        //            break;
        //        default: throw new Exception($"Option {selectedOption} not supported");
        //    };
        //}



        //public void AdminMenu()
        //{
        //    Console.Clear();

        //    Console.WriteLine("\nMenu: please select option:");
        //    Console.WriteLine("1. Search by login");
        //    Console.WriteLine("2. All user list");
        //    Console.WriteLine("3. Add a new currency");
        //    Console.WriteLine("4. Delete the currency");
        //    Console.WriteLine("5. Add money to stock");
        //    Console.WriteLine("6. Show available money on stock");
        //    Console.WriteLine("7. Add Currencies from NBP API");
        //    Console.WriteLine("8. Exchange calculator");
        //    Console.WriteLine("9. Exchange Office Transaction History");
        //    Console.WriteLine("10. Exit");
        //    Console.WriteLine("You choose: ");

        //    int selectedOption = MenuOptionsHelper.GetUserSelectionAndValidate(1, 10);


        //    switch (selectedOption)
        //    {
        //        case 1:
        //            _menuAdminActions.SearchByLogin();
        //            break;
        //        case 2:
        //            _menuAdminActions.ListAllUsers();
        //            break;
        //        case 3:
        //            _menuAdminActions.AddCurrency();
        //            break;
        //        case 4:
        //            PrintExchangeOfficeBoard();
        //            _menuAdminActions.DeleteCurrency();
        //            break;
        //        case 5:
        //            _menuAdminActions.AddMoneyToStock();
        //            break;
        //        case 6:
        //            _menuAdminActions.ShowAvailableMoneyOnStock();
        //            break;
        //        case 7:
        //            PrintExchangeOfficeBoard();
        //            _menuAdminActions.AddCurrencyFromNbpApi();
        //            PrintExchangeOfficeBoard();
        //            break;
        //        case 8:
        //            _menuAdminActions.CurrencyCalculate();
        //            break;
        //        case 9:
        //            _menuAdminActions.TransactionHistory();
        //            break;
        //        case 10:
        //            WelcomeMenu();
        //            break;
        //        default: throw new Exception($"Option {selectedOption} not supported");
        //    };

        //    var toMainMenu = MenuOptionsHelper.GetBackToMainMenuQuestion();

        //    if (toMainMenu)
        //    {
        //        AdminMenu();
        //    }
        //    else
        //    {
        //        WelcomeMenu();
        //    }
        //}

        //public void UserMenu()
        //{
        //    Console.Clear();

        //    Console.WriteLine("\nMenu: please select option:");
        //    Console.WriteLine("1. Deposit");
        //    Console.WriteLine("2. Withdrawal");
        //    Console.WriteLine("3. Exchange");
        //    Console.WriteLine("4. History");
        //    Console.WriteLine("5. Currency calculator");
        //    Console.WriteLine("6. Account balance");
        //    Console.WriteLine("7. Exit");
        //    Console.WriteLine("You choose: ");

        //    int selectedOption = MenuOptionsHelper.GetUserSelectionAndValidate(1, 7);

        //    switch (selectedOption)
        //    {
        //        case 1:
        //            Console.WriteLine("Navigate to: Deposit");
        //            _menuUserActions.Deposit(CurrentUser);
        //            break;
        //        case 2:
        //            Console.WriteLine("Navigate to: Withdrawal");
        //            _menuUserActions.Withdrawal(CurrentUser);
        //            break;
        //        case 3:
        //            Console.WriteLine("Navigate to: Exchange");
        //            _menuUserActions.Exchange(CurrentUser);
        //            break;
        //        case 4:
        //            Console.WriteLine("Navigate to: History");
        //            _menuUserActions.GetUserHistory(CurrentUser);
        //            break;
        //        case 5:
        //            //Console.WriteLine("Navigate to: Currency calculator");
        //            _menuAdminActions.CurrencyCalculate();
        //            break;
        //        case 6:
        //            _menuUserActions.GetWallet(CurrentUser);
        //            break;
        //        case 7:
        //            WelcomeMenu();
        //            break;
        //        default: throw new Exception($"Option {selectedOption} not supported");
        //    };

        //    var toMainMenu = MenuOptionsHelper.GetBackToMainMenuQuestion();

        //    if (toMainMenu)
        //    {
        //        UserMenu();
        //    }
        //    else
        //    {
        //        WelcomeMenu();
        //    }
        //}

        //private void PrintExchangeOfficeBoard(string searchBy = null)
        //{
        //    Console.Clear();
        //    List<Currency> exchangeOfficeBoard;
        //    if (searchBy is null)
        //    {
        //        exchangeOfficeBoard = _exchangeOfficeBoardSevice.GetAllCurrencies();
        //        Console.WriteLine("*=== Exchange Office Board ===*");

        //    }
        //    else
        //    {

        //        exchangeOfficeBoard = _exchangeOfficeBoardSevice.BrowseCurrency(searchBy);
        //        Console.WriteLine($"Result: {exchangeOfficeBoard.Count()}");
        //    }

        //    Console.WriteLine("| BuyAt | SellAt | ShortName | Name               | Country");
        //    for (int i = 0; i < exchangeOfficeBoard.Count(); i++)
        //    {
        //        WriteAt($"| {exchangeOfficeBoard[i].BuyAt}", 0, i + 2);
        //        WriteAt($"| {exchangeOfficeBoard[i].SellAt}", 8, i + 2);
        //        WriteAt($"| {exchangeOfficeBoard[i].ShortName}", 17, i + 2);
        //        WriteAt($"| {exchangeOfficeBoard[i].Name}", 29, i + 2);
        //        WriteAt($"| {exchangeOfficeBoard[i].Country}", 50, i + 2);
        //        Console.WriteLine();
        //    }

        //}

        //private void PrintNBPRates(string searchBy = null)
        //{
        //    Console.Clear();
        //    List<Currency> nbpRates;
        //    if (searchBy is null)
        //    {
        //        nbpRates = _nbpJsonService.GetAllCurrencies();
        //        var nbpTableDescription = _nbpJsonService.GetNBPJsonTableInfo();
        //        Console.WriteLine("*=== NBP Exchangerates Table C ===*");
        //        Console.WriteLine($"** Table: {nbpTableDescription.table} No: {nbpTableDescription.no} **");
        //        Console.WriteLine($"Effective Date: {nbpTableDescription.effectiveDate}");

        //    }
        //    else
        //    {

        //        nbpRates = _nbpJsonService.BrowseCurrency(searchBy);
        //        Console.WriteLine($"Result: {nbpRates.Count()}");
        //    }

        //    Console.WriteLine("| Ask | Bid | Code | Currency");
        //    for (int i = 0; i < nbpRates.Count(); i++)
        //    {
        //        WriteAt($"| {nbpRates[i].BuyAt}", 0, i + 2);
        //        WriteAt($"| {nbpRates[i].SellAt}", 8, i + 2);
        //        WriteAt($"| {nbpRates[i].ShortName}", 17, i + 2);
        //        WriteAt($"| {nbpRates[i].Name}", 29, i + 2);
        //        WriteAt($"| {nbpRates[i].Country}", 50, i + 2);
        //        Console.WriteLine();
        //    }
        //}

        //private void ExchangeOfficeBoardMenu(string menu)
        //{

        //    Console.WriteLine("\nMenu:");
        //    Console.WriteLine("1. Search for currency");
        //    Console.WriteLine("2. Show all curriences");
        //    Console.WriteLine("3. Exit");
        //    Console.WriteLine("You choose: ");

        //    int selectedOption = MenuOptionsHelper.GetUserSelectionAndValidate(1, 3);

        //    string searchBy;

        //    switch (selectedOption)
        //    {
        //        case 1:
        //            Console.Write("searching... type name, shortname or country:");
        //            searchBy = MenuOptionsHelper.CheckString(Console.ReadLine());
        //            if (menu.Equals("Exchange Office Board"))
        //            {
        //                PrintExchangeOfficeBoard(searchBy);
        //                ExchangeOfficeBoardMenu("Exchange Office Board");
        //            }
        //            else
        //            {
        //                PrintNBPRates(searchBy);
        //                ExchangeOfficeBoardMenu("NBP Today Rates");
        //            }
        //            break;
        //        case 2:
        //            Console.Clear();
        //            if (menu.Equals("Exchange Office Board"))
        //            {
        //                PrintExchangeOfficeBoard();
        //                ExchangeOfficeBoardMenu("Exchange Office Board");
        //            }
        //            else
        //            {
        //                PrintNBPRates();
        //                ExchangeOfficeBoardMenu("NBP Today Rates");
        //            }
        //            break;
        //        case 3:
        //            Console.Clear();
        //            WelcomeMenu();
        //            break;
        //        default: throw new Exception($"Option {selectedOption} not supported");
        //    };
        //}

        //private void WriteAt(string s, int x, int y)
        //{
        //    int origRow = 0;
        //    int origCol = 0;
        //    try
        //    {
        //        Console.SetCursorPosition(origCol + x, origRow + y);
        //        Console.Write(s);
        //    }
        //    catch (ArgumentOutOfRangeException e)
        //    {
        //        Console.Clear();
        //        Console.WriteLine(e.Message);
        //    }
        //}
    }
}
