using VistulaExchange.Database.Domain;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VistulaExchange.Web.Controllers
{
    public class UserWithdrawalController : Controller
    {
        private readonly IUserWalletService _userWalletService;

        public UserWithdrawalController(IUserWalletService userWalletService)
        {
            _userWalletService = userWalletService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var userWallet = await _userWalletService.GetWalletAsync(userId);
            var userWithdrawal = userWallet.WalletPositions.ToList()
                .Select(a => new UserWithdrawal()
                {
                    Currency = a.Currency.ShortName,
                    Amount = a.CurrencyAmount
                });

            return View(userWithdrawal);
        }

        public async Task<IActionResult> WithdrawalIndex(string currency)
        {
            var currencyAmount = await GetCurrencyBalanceAsync(currency);

            var userWithdrawal = new UserWithdrawal();
            userWithdrawal.Currency = currency;
            userWithdrawal.Amount = currencyAmount.CurrencyAmount;

            return View("Withdrawal", userWithdrawal);
        }

        public async Task<IActionResult> Withdrawal(UserWithdrawal userWithdrawal)
        {
            var currentBalance = await GetCurrencyBalanceAsync(userWithdrawal.Currency);

            if (userWithdrawal.Amount > currentBalance.CurrencyAmount)
            {
                ViewData["errorMessage"] = "Requested currency withdrawal amount larger than current saldo.";
                ViewData["currentBalance"] = currentBalance.CurrencyAmount;
                return View("Withdrawal", userWithdrawal);
            }
            // Check balance after withdrawal

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            await _userWalletService.WithdrawalAsync(userId, userWithdrawal.Currency, userWithdrawal.Amount, "Withdrawal");

            var newBalance = await GetCurrencyBalanceAsync(userWithdrawal.Currency);
            ViewData["currentSaldo"] = newBalance.CurrencyAmount;
            ViewData["currenncy"] = userWithdrawal.Currency;
            ViewData["activePage"] = "UserWithdrawal";

            return View("WithdrawalCompleted");
        }

        private async Task<WalletPosition> GetCurrencyBalanceAsync(string currency)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new WalletPosition();
            }

            var currencyAmount = await _userWalletService.GetCurrencyBalanceByCodeAsync(userId, currency);
            return currencyAmount ?? new WalletPosition();
        }
    }
}
