using VistulaExchange.Database.Domain;
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
        private readonly IUserWalletService _userWalletService;
        private readonly IExchangeOfficeBoardService _exchangeOfficeBoardService;

        public UserDepositController(IUserWalletService userWalletService, IExchangeOfficeBoardService exchangeOfficeBoardService)
        {
            _userWalletService = userWalletService;
            _exchangeOfficeBoardService = exchangeOfficeBoardService;
        }

        public IActionResult Index()
        {
            ViewData["activePage"] = "UserDeposit";
            ViewData["currencies"] = _exchangeOfficeBoardService.GetAllCurrencies().Select(a => a.ShortName);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(UserDeposit userDeposit)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            await _userWalletService.DepositAsync(userId, userDeposit.Currency, userDeposit.Amount, "Deposit");

            var wallet = await _userWalletService.GetWalletAsync(userId);
            var currentSaldo = wallet.WalletPositions.SingleOrDefault(a => a.Currency.ShortName == userDeposit.Currency);

            ViewData["currentSaldo"] = currentSaldo?.CurrencyAmount ?? 0;
            ViewData["currency"] = userDeposit.Currency;
            ViewData["activePage"] = "UserDeposit";

            return View("DepositCompleted");
        }
    }
}
