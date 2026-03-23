using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VistulaExchange.Web.Controllers
{
    [Authorize]
    public class UserDepositController : Controller
    {
        private readonly IUserWalletRepository _userWalletRepository;
        private readonly IExchangeOfficeBoardService _exchangeOfficeBoardService;

        public UserDepositController(IUserWalletRepository userWalletRepository, IExchangeOfficeBoardService exchangeOfficeBoardService)
        {
            _userWalletRepository = userWalletRepository;
            _exchangeOfficeBoardService = exchangeOfficeBoardService;
        }

        public IActionResult Index()
        {
            ViewData["activePage"] = "UserDeposit";
            ViewData["currencies"] = _exchangeOfficeBoardService.GetAllCurrencies().Select(a => a.ShortName);
            return View();
        }

        public IActionResult Deposit(UserDeposit userDeposit)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            _userWalletRepository.Deposit(userId, userDeposit.Currency, userDeposit.Amount, "Deposit");

            var wallet = _userWalletRepository.GetWallet(userId);
            var currentSaldo = wallet.WalletPositions.SingleOrDefault(a => a.Currency.ShortName == userDeposit.Currency);

            ViewData["currentSaldo"] = currentSaldo?.CurrencyAmount ?? 0;
            ViewData["currenncy"] = userDeposit.Currency;
            ViewData["activePage"] = "UserDeposit";

            return View("DepositCompleted");
        }
    }
}
