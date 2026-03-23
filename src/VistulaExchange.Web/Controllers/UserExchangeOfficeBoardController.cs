using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VistulaExchange.Database.Domain;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Web.Models;
using System.Text.Json;

namespace VistulaExchange.Web.Controllers
{
    public class UserExchangeOfficeBoardController : Controller
    {
        private readonly IExchangeOfficeBoardService _exchangeOfficeBoardService;
        private readonly IUserWalletService _userWalletService;
        private readonly IAvailableMoneyOnStockService _availableMoneyOnStockService;
        private readonly ITradeQuoteService _tradeQuoteService;
        private readonly INbpJsonService _nbpJsonService;

        public UserExchangeOfficeBoardController(
            IExchangeOfficeBoardService exchangeOfficeBoardService,
            IUserWalletService userWalletService,
            IAvailableMoneyOnStockService availableMoneyOnStockService,
            ITradeQuoteService tradeQuoteService,
            INbpJsonService nbpJsonService)
        {
            _exchangeOfficeBoardService = exchangeOfficeBoardService;
            _userWalletService = userWalletService;
            _availableMoneyOnStockService = availableMoneyOnStockService;
            _tradeQuoteService = tradeQuoteService;
            _nbpJsonService = nbpJsonService;
        }

        public ActionResult Index(string? query)
        {
            ViewData["activePage"] = "UserExchangeOfficeBoard";

            var source = string.IsNullOrWhiteSpace(query)
                ? _exchangeOfficeBoardService.GetAllCurrencies()
                : _exchangeOfficeBoardService.BrowseCurrency(query.Trim());

            var model = new ExchangeBoardViewModel
            {
                Title = "Live Exchange Board",
                Subtitle = "Scan the market, compare spreads and jump straight into buy or sell workflows.",
                SearchQuery = query?.Trim() ?? string.Empty,
                IsAdminView = false,
                Currencies = source
                    .Where(m => !string.Equals(m.ShortName, "PLN", StringComparison.OrdinalIgnoreCase))
                    .Select(CurrencyCardViewModel.FromCurrency)
                    .OrderBy(m => m.ShortName)
                    .ToList()
            };

            return View(model);
        }

        public ActionResult Details(Guid id)
        {
            ViewData["activePage"] = "UserExchangeOfficeBoard";
            var currency = _exchangeOfficeBoardService.GetCurrencyById(id);
            if (currency is null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(new CurrencyDetailsViewModel
            {
                Id = currency.Id,
                Name = currency.Name,
                ShortName = currency.ShortName,
                Country = currency.Country,
                BuyAt = currency.BuyAt,
                SellAt = currency.SellAt
            });
        }

        [HttpGet]
        public async Task<ActionResult> Buy(Guid id)
        {
            ViewData["activePage"] = "UserExchangeOfficeBoard";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var quote = await _tradeQuoteService.BuildQuoteAsync(userId, id, "Buy");
            if (quote is null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(MapQuote(quote));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Buy(QuickExchangeViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            if (model.QuoteExpired)
            {
                return await ReturnBuyErrorAsync(userId, model, "Quote lock expired. Refresh the deal to get a fresh rate.");
            }

            if (model.RequestedAmount <= 0 || model.RequestedAmount > model.MaxAmount)
            {
                return await ReturnBuyErrorAsync(userId, model, "Requested amount is above the available balance or desk liquidity.");
            }

            var subtractResult = await _availableMoneyOnStockService.SubtractMoneyFromStockAsync(new MoneyOnStock
            {
                CurrencyName = model.TargetCurrencyCode,
                Value = model.RequestedAmount
            });

            if (!string.IsNullOrWhiteSpace(subtractResult))
            {
                return await ReturnBuyErrorAsync(userId, model, subtractResult);
            }

            await _availableMoneyOnStockService.AddMoneyToStockAsync(new MoneyOnStock
            {
                CurrencyName = model.SourceCurrencyCode,
                Value = model.NetValue
            });

            await _userWalletService.WithdrawalAsync(userId, model.SourceCurrencyCode, model.NetValue, "Buy");
            await _userWalletService.DepositAsync(userId, model.TargetCurrencyCode, model.RequestedAmount, "Buy");

            var newBalance = (await _userWalletService.GetCurrencyBalanceByCodeAsync(userId, model.SourceCurrencyCode)).CurrencyAmount;

            ViewData["currentSaldo"] = newBalance;
            ViewData["currency"] = model.SourceCurrencyCode;
            ViewData["activePage"] = "UserBuy";

            return View("BuyCompleted");
        }

        [HttpGet]
        public async Task<ActionResult> Sell(Guid id)
        {
            ViewData["activePage"] = "UserExchangeOfficeBoard";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var quote = await _tradeQuoteService.BuildQuoteAsync(userId, id, "Sell");
            if (quote is null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(MapQuote(quote));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Sell(QuickExchangeViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            if (model.QuoteExpired)
            {
                return await ReturnSellErrorAsync(userId, model, "Quote lock expired. Refresh the deal to get a fresh rate.");
            }

            if (model.RequestedAmount <= 0 || model.RequestedAmount > model.MaxAmount)
            {
                return await ReturnSellErrorAsync(userId, model, "Requested amount is above the available balance or desk liquidity.");
            }

            var subtractResult = await _availableMoneyOnStockService.SubtractMoneyFromStockAsync(new MoneyOnStock
            {
                CurrencyName = model.TargetCurrencyCode,
                Value = model.NetValue
            });

            if (!string.IsNullOrWhiteSpace(subtractResult))
            {
                return await ReturnSellErrorAsync(userId, model, subtractResult);
            }

            await _availableMoneyOnStockService.AddMoneyToStockAsync(new MoneyOnStock
            {
                CurrencyName = model.SourceCurrencyCode,
                Value = model.RequestedAmount
            });

            await _userWalletService.WithdrawalAsync(userId, model.SourceCurrencyCode, model.RequestedAmount, "Sell");
            await _userWalletService.DepositAsync(userId, model.TargetCurrencyCode, model.NetValue, "Sell");

            var newBalance = (await _userWalletService.GetCurrencyBalanceByCodeAsync(userId, model.TargetCurrencyCode)).CurrencyAmount;
            ViewData["currentSaldo"] = newBalance;
            ViewData["currency"] = model.TargetCurrencyCode;
            ViewData["activePage"] = "UserSell";

            return View("SellCompleted");
        }

        [HttpGet]
        public async Task<IActionResult> History(string currencyCode, int days = 30)
        {
            var safeDays = Math.Clamp(days, 7, 90);
            var safeCode = (currencyCode ?? string.Empty).Trim().ToUpperInvariant();
            if (string.IsNullOrWhiteSpace(safeCode))
            {
                return BadRequest();
            }

            try
            {
                var history = await _nbpJsonService.GetCurrencyHistoryAsync(safeCode, safeDays);
                return Json(history.Select(point => new
                {
                    date = point.Date.ToString("yyyy-MM-dd"),
                    buyAt = point.BuyAt,
                    sellAt = point.SellAt
                }));
            }
            catch
            {
                return Json(Array.Empty<object>());
            }
        }

        private async Task<ActionResult> ReturnBuyErrorAsync(string userId, QuickExchangeViewModel model, string errorMessage)
        {
            ViewData["activePage"] = "UserExchangeOfficeBoard";
            var refreshedQuote = await _tradeQuoteService.BuildQuoteAsync(userId, model.CurrencyId, "Buy", model.RequestedAmount);
            if (refreshedQuote is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = MapQuote(refreshedQuote);
            viewModel.ErrorMessage = errorMessage;
            return View("Buy", viewModel);
        }

        private async Task<ActionResult> ReturnSellErrorAsync(string userId, QuickExchangeViewModel model, string errorMessage)
        {
            ViewData["activePage"] = "UserExchangeOfficeBoard";
            var refreshedQuote = await _tradeQuoteService.BuildQuoteAsync(userId, model.CurrencyId, "Sell", model.RequestedAmount);
            if (refreshedQuote is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = MapQuote(refreshedQuote);
            viewModel.ErrorMessage = errorMessage;
            return View("Sell", viewModel);
        }

        private static QuickExchangeViewModel MapQuote(VistulaExchange.Services.Models.TradeQuoteDto quote)
        {
            return new QuickExchangeViewModel
            {
                CurrencyId = quote.CurrencyId,
                CurrencyName = quote.CurrencyName,
                Mode = quote.Mode,
                SourceCurrencyCode = quote.SourceCurrencyCode,
                TargetCurrencyCode = quote.TargetCurrencyCode,
                SourceBalance = quote.SourceBalance,
                TargetBalance = quote.TargetBalance,
                RequestedAmount = quote.RequestedAmount,
                MaxAmount = quote.MaxAmount,
                Rate = quote.Rate,
                FeeRate = quote.FeeRate,
                GrossValue = quote.GrossValue,
                FeeAmount = quote.FeeAmount,
                NetValue = quote.NetValue,
                TargetLiquidityAvailable = quote.TargetLiquidityAvailable,
                QuoteLockedUntilUtc = quote.QuoteLockedUntilUtc
            };
        }
    }
}
