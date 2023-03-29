using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JediApp.Web.Controllers
{
    public class UserWithdrawalController : Controller
    {
        private readonly IUserWalletRepository _userWalletkRepository;

        public UserWithdrawalController(IUserWalletRepository userWalletkRepository)
        {
            _userWalletkRepository = userWalletkRepository;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userWallet = _userWalletkRepository.GetWallet(userId);
            var userWithdrawal = userWallet.WalletPositions.ToList()
                .Select(a => new UserWithdrawal()
                {
                    Currency = a.Currency.ShortName,
                    Amount = a.CurrencyAmount
                });

            return View(userWithdrawal);
        }

        public IActionResult WithdrawalIndex(string currency)
        {
            var currencyAmount = GetCurrencyBalance(currency);

            var userWithdrawal = new UserWithdrawal();
            userWithdrawal.Currency = currency;
            userWithdrawal.Amount = currencyAmount.CurrencyAmount;

            return View("Withdrawal", userWithdrawal);
        }

        public IActionResult Withdrawal(UserWithdrawal userWithdrawal)
        {
            var currentBalance = GetCurrencyBalance(userWithdrawal.Currency);

            if (userWithdrawal.Amount > currentBalance.CurrencyAmount)
            {
                ViewData["errorMessage"] = "Requested currency withdrawal amount larger than current saldo.";
                ViewData["currentBalance"] = currentBalance.CurrencyAmount;
                return View("Withdrawal", userWithdrawal);
            }
            // Check balance after withdrawal

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _userWalletkRepository.Withdrawal(userId, userWithdrawal.Currency, userWithdrawal.Amount, "Withdrawal");

            var newBalance = GetCurrencyBalance(userWithdrawal.Currency);
            ViewData["currentSaldo"] = newBalance.CurrencyAmount;
            ViewData["currenncy"] = userWithdrawal.Currency;
            ViewData["activePage"] = "UserWithdrawal";

            return View("WithdrawalCompleted");
        }

        private WalletPosition GetCurrencyBalance(string currency)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wallet = _userWalletkRepository.GetWallet(userId);
            var currencyAmount = wallet.WalletPositions.Where(a => a.Currency.ShortName == currency).Single();
            return currencyAmount;
        }
    }
}
