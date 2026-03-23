using VistulaExchange.Database.Domain;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace VistulaExchange.Web.Controllers
{
    public class MoneyStockController : Controller
    {
        private readonly IAvailableMoneyOnStockService _availableMoneyOnStockService;

        public MoneyStockController(IAvailableMoneyOnStockService availableMoneyOnStockService)
        {
            _availableMoneyOnStockService = availableMoneyOnStockService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["activePage"] = "MoneyStock";

            var moneyOnStock = (await _availableMoneyOnStockService.GetAvailableMoneyOnStockAsync())
                .Select(a => new AdminDeposit
                {
                    Currency = a.CurrencyName,
                    Amount = a.Value
                });

            return View(moneyOnStock);
        }

        public IActionResult DepositIndex()
        {
            ViewData["activePage"] = "MoneyStock";

            return View("Deposit");
        }

        public async Task<IActionResult> Deposit(AdminDeposit adminDeposit)
        {
            var depositDetails = new MoneyOnStock
            {
                CurrencyName = adminDeposit.Currency,
                Value = adminDeposit.Amount
            };

            await _availableMoneyOnStockService.AddMoneyToStockAsync(depositDetails);

            var moneyOnStock = await _availableMoneyOnStockService.GetAvailableMoneyOnStockAsync();
            var currentSaldo = moneyOnStock.SingleOrDefault(a => a.CurrencyName == adminDeposit.Currency);

            ViewData["currentSaldo"] = currentSaldo?.Value ?? 0;
            ViewData["currenncy"] = adminDeposit.Currency;
            ViewData["activePage"] = "MoneyStock";

            return View("DepositCompleted");
        }


    }
}
