using VistulaExchange.Database.Domain;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace VistulaExchange.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExchangeOfficeBoardController : Controller
    {
        private readonly IExchangeOfficeBoardService _exchangeOfficeBoardService;
        private readonly INbpJsonService _nbpJsonService;
        private readonly IExchangeOfficeService _exchangeOfficeService;
        private readonly IUserAlarmsService _userAlarmService;

        public ExchangeOfficeBoardController(IExchangeOfficeBoardService exchangeOfficeBoardService, INbpJsonService nbpJsonService, IExchangeOfficeService exchangeOfficeService, IUserAlarmsService userAlarmService)
        {
            _exchangeOfficeBoardService = exchangeOfficeBoardService;
            _nbpJsonService = nbpJsonService;
            _exchangeOfficeService = exchangeOfficeService;
            _userAlarmService = userAlarmService;
        }

        public ActionResult Index(string? query)
        {
            ViewData["activePage"] = "AdminExchange";

            var source = string.IsNullOrWhiteSpace(query)
                ? _exchangeOfficeBoardService.GetAllCurrencies()
                : _exchangeOfficeBoardService.BrowseCurrency(query.Trim());

            var model = new ExchangeBoardViewModel
            {
                Title = "Trading Desk",
                Subtitle = "Manage rates, refresh the market feed and steer the spread from one place.",
                SearchQuery = query?.Trim() ?? string.Empty,
                IsAdminView = true,
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
            var currency = _exchangeOfficeBoardService.GetCurrencyById(id);
            return View(currency);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Currency newCurrency)
        {
            if (!ModelState.IsValid)
            {
                return View(newCurrency);
            }

            _exchangeOfficeBoardService.AddCurrency(newCurrency);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(Guid id)
        {
            var currency = _exchangeOfficeBoardService.GetCurrencyById(id);
            if (currency != null)
            {
                return View(currency);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, Currency currency)
        {
            if (!ModelState.IsValid)
            {
                return View(currency);
            }

            var currentCurrency = _exchangeOfficeBoardService.GetCurrencyById(id);
            if (currentCurrency == null)
            {
                return RedirectToAction(nameof(Index));
            }

            _exchangeOfficeBoardService.UpdateCurrency(id, currency);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(Guid id)
        {
            var currency = _exchangeOfficeBoardService.GetCurrencyById(id);

            return View(currency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            var currencyToDelete = _exchangeOfficeBoardService.GetCurrencyById(id);
            if (currencyToDelete == null)
            {
                return RedirectToAction(nameof(Index));
            }

            _exchangeOfficeBoardService.DeleteCurrencyByShortName(currencyToDelete.ShortName);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCurrencyFromNbpApi()
        {

            List<Currency> nbpCurrencies = await _nbpJsonService.GetAllCurrenciesAsync();

            foreach (var currency in nbpCurrencies)
            {
                _exchangeOfficeBoardService.AddCurrency(new Currency()
                {
                    Name = currency.Name,
                    ShortName = currency.ShortName,
                    Country = currency.Country,
                    BuyAt = currency.BuyAt,
                    SellAt = currency.SellAt
                });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateCurrencyRatesFromNbpApi()
        {

            List<Currency> nbpCurrencies = await _nbpJsonService.GetAllCurrenciesAsync();
            var currencies = _exchangeOfficeBoardService.GetAllCurrencies();

            foreach (var currency in currencies)
            {
                try
                {
                    if (!string.Equals(currency.ShortName, "PLN", StringComparison.OrdinalIgnoreCase))
                    {
                        var currencyToUpdate = nbpCurrencies.FirstOrDefault(nc =>
                            string.Equals(nc.ShortName, currency.ShortName, StringComparison.OrdinalIgnoreCase));
                        if (currencyToUpdate != null)
                        {
                            currency.BuyAt = currencyToUpdate.BuyAt;
                            currency.SellAt = currencyToUpdate.SellAt;
                            _exchangeOfficeBoardService.UpdateCurrency(currency.Id, currency);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"[ExchangeOfficeBoardController] Failed to update currency {currency.ShortName}: {ex.Message}");
                }

            }

            await _userAlarmService.ExecuteAlarmsAsync(currencies);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Markup()
        {
            var currencies = _exchangeOfficeBoardService.GetAllCurrencies();
            var exchangeOffice = _exchangeOfficeService.GetAllExchangeOffices().FirstOrDefault();
            if (exchangeOffice == null)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var currency in currencies)
            {
                var markup = exchangeOffice.Markup;
                currency.BuyAt *= 1 + (decimal)markup / 200;
                currency.SellAt *= 1 - (decimal)markup / 200;
                _exchangeOfficeBoardService.UpdateCurrency(currency.Id, currency);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
