using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Services.Services.Interfaces;
using JediApp.Web.Areas.Identity.Data;
using JediApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;

namespace JediApp.Web.Controllers
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

            // Perform deposit
            _userWalletRepository.Deposit(userId, userDeposit.Currency, userDeposit.Amount, "Deposit");

            // Check balance after deposit
            var wallet = _userWalletRepository.GetWallet(userId);
            var currentSaldo = wallet.WalletPositions.Where(a => a.Currency.ShortName == userDeposit.Currency).SingleOrDefault();

            ViewData["currentSaldo"] = currentSaldo.CurrencyAmount;
            ViewData["currenncy"] = userDeposit.Currency;
            ViewData["activePage"] = "UserDeposit";

            return View("DepositCompleted");
        }
    }
}
