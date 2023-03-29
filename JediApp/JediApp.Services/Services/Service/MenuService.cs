using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Services.Services.Interfaces;

namespace JediApp.Services.Services.Service
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
    }
}
