using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Web.Models;

namespace VistulaExchange.Web.Controllers
{
    public class UserExchangeOfficeBoardController : Controller
    {
        private readonly IExchangeOfficeBoardService _exchangeOfficeBoardService;
        private readonly IUserWalletService _userWalletService;
        private readonly IAvailableMoneyOnStockRepository _availableMoneyOnStockRepository;

        public UserExchangeOfficeBoardController(
            IExchangeOfficeBoardService exchangeOfficeBoardService,
            IUserWalletService userWalletService,
            IAvailableMoneyOnStockRepository availableMoneyOnStockRepository)
        {
            _exchangeOfficeBoardService = exchangeOfficeBoardService;
            _userWalletService = userWalletService;
            _availableMoneyOnStockRepository = availableMoneyOnStockRepository;
        }

        public ActionResult Index()
        {
            ViewData["activePage"] = "UserExchangeOfficeBoard";

            var model = _exchangeOfficeBoardService.GetAllCurrencies();
            var pln = model.FirstOrDefault(m => string.Equals(m.ShortName, "PLN", StringComparison.OrdinalIgnoreCase));
            if (pln != null)
            {
                model.Remove(pln);
            }

            return View(model);
        }

        public ActionResult Details(Guid id)
        {
            var currency = _exchangeOfficeBoardService.GetCurrencyById(id);
            return View(currency);
        }

        public ActionResult Buy(Guid id)
        {
            var currency = _exchangeOfficeBoardService.GetCurrencyById(id);
            var exchangeFromCurrency = _exchangeOfficeBoardService
                .GetAllCurrencies()
                .FirstOrDefault(c => string.Equals(c.ShortName, "PLN", StringComparison.OrdinalIgnoreCase));
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            if (currency == null || exchangeFromCurrency == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var exchangeFromCurrencyAmount =
                _userWalletService.GetCurrencyBalanceById(userId, exchangeFromCurrency.Id)?.CurrencyAmount ?? 0m;

            var userExchange = new UserExchange
            {
                ExchangeFromCurrency = exchangeFromCurrency.ShortName,
                ExchangeFromCurrencyAmount = exchangeFromCurrencyAmount,
                ExchangeToCurrency = currency.ShortName,
                ExchangeToCurrencyAmount = 0,
                BuyAt = currency.BuyAt,
                SellAt = currency.SellAt,
                ExchangeMaxAmount = exchangeFromCurrencyAmount / currency.BuyAt
            };

            return View(userExchange);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Buy(UserExchange userExchange)
        {
            if (userExchange.ExchangeToCurrencyAmount > userExchange.ExchangeMaxAmount)
            {
                ViewData["errorMessage"] = "Requested currency buy amount larger than current saldo.";
                ViewData["currentBalance"] = userExchange.ExchangeMaxAmount;
                return View("Buy", userExchange);
            }

            var result = _availableMoneyOnStockRepository.SubtractMoneyFromStock(new MoneyOnStock
            {
                CurrencyName = userExchange.ExchangeToCurrency,
                Value = userExchange.ExchangeToCurrencyAmount
            });

            if (!string.IsNullOrWhiteSpace(result))
            {
                ViewData["errorMessage"] = result;
                ViewData["currentBalance"] = userExchange.ExchangeMaxAmount;
                return View("Buy", userExchange);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            _userWalletService.Withdrawal(userId, userExchange.ExchangeFromCurrency, userExchange.ExchangeToCurrencyAmount * userExchange.BuyAt, "Buy");
            _userWalletService.Deposit(userId, userExchange.ExchangeToCurrency, userExchange.ExchangeToCurrencyAmount, "Buy");

            var exchangeFromCurrency = _exchangeOfficeBoardService
                .GetAllCurrencies()
                .FirstOrDefault(c => string.Equals(c.ShortName, userExchange.ExchangeFromCurrency, StringComparison.OrdinalIgnoreCase));
            if (exchangeFromCurrency == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var newBalance = _userWalletService.GetCurrencyBalanceById(userId, exchangeFromCurrency.Id)?.CurrencyAmount ?? 0m;

            ViewData["currentSaldo"] = newBalance;
            ViewData["currency"] = userExchange.ExchangeFromCurrency;
            ViewData["activePage"] = "UserBuy";

            return View("BuyCompleted");
        }

        public ActionResult Sell(Guid id)
        {
            var currency = _exchangeOfficeBoardService
                .GetAllCurrencies()
                .FirstOrDefault(c => string.Equals(c.ShortName, "PLN", StringComparison.OrdinalIgnoreCase));
            var exchangeFromCurrency = _exchangeOfficeBoardService.GetCurrencyById(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            if (currency == null || exchangeFromCurrency == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var exchangeFromCurrencyAmount = _userWalletService.GetCurrencyBalanceById(userId, id)?.CurrencyAmount ?? 0m;

            var userExchange = new UserExchange
            {
                ExchangeFromCurrency = exchangeFromCurrency.ShortName,
                ExchangeFromCurrencyAmount = exchangeFromCurrencyAmount,
                ExchangeToCurrency = currency.ShortName,
                ExchangeToCurrencyAmount = _userWalletService.GetCurrencyBalanceById(userId, currency.Id)?.CurrencyAmount ?? 0m,
                BuyAt = currency.BuyAt,
                SellAt = currency.SellAt,
                ExchangeMaxAmount = exchangeFromCurrencyAmount
            };

            return View(userExchange);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sell(UserExchange userExchange)
        {
            if (userExchange.ExchangeFromCurrencyAmount > userExchange.ExchangeMaxAmount)
            {
                ViewData["errorMessage"] = "Requested currency buy amount larger than current saldo.";
                ViewData["currentBalance"] = userExchange.ExchangeMaxAmount;
                return View("Sell", userExchange);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            _userWalletService.Withdrawal(userId, userExchange.ExchangeFromCurrency, userExchange.ExchangeFromCurrencyAmount, "Sell");
            _userWalletService.Deposit(userId, userExchange.ExchangeToCurrency, userExchange.ExchangeFromCurrencyAmount * userExchange.SellAt, "Sell");

            var exchangeToCurrency = _exchangeOfficeBoardService
                .GetAllCurrencies()
                .FirstOrDefault(c => string.Equals(c.ShortName, userExchange.ExchangeToCurrency, StringComparison.OrdinalIgnoreCase));
            if (exchangeToCurrency == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var newBalance = _userWalletService.GetCurrencyBalanceById(userId, exchangeToCurrency.Id)?.CurrencyAmount ?? 0m;
            ViewData["currentSaldo"] = newBalance;
            ViewData["currency"] = userExchange.ExchangeToCurrency;
            ViewData["activePage"] = "UserSell";

            return View("SellCompleted");
        }
    }
}
