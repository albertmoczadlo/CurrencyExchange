using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Interfaces;

namespace VistulaExchange.Services.Services.Service
{
    public class MenuService
    {
        private readonly IUserService _userService;
        private readonly MenuRoleUserService _menuUserActions;
        private readonly MenuRoleAdminService _menuAdminActions;
        private readonly IExchangeOfficeBoardService _exchangeOfficeBoardSevice;
        private readonly INbpJsonService _nbpJsonService;
        private readonly ITransactionHistoryService _transactionHistoryService;

        public MenuService(IUserService userService, IUserWalletRepository userWalletRepository, IExchangeOfficeBoardService exchangeOfficeBoardSevice, IAvailableMoneyOnStockService availableMoneyOnStockService, INbpJsonService nbpJsonService, ITransactionHistoryService transactionHistoryService)

        {
            _userService = userService;
            _nbpJsonService = nbpJsonService;
            _exchangeOfficeBoardSevice = exchangeOfficeBoardSevice;
            _transactionHistoryService = transactionHistoryService;
            _menuUserActions = new MenuRoleUserService(_userService, userWalletRepository, _exchangeOfficeBoardSevice, _transactionHistoryService);
            _menuAdminActions = new MenuRoleAdminService(_userService, _exchangeOfficeBoardSevice, availableMoneyOnStockService, _nbpJsonService, _transactionHistoryService);

        }
    }
}
