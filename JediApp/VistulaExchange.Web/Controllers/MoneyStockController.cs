using VistulaExchange.Database.Interface;
using VistulaExchange.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace VistulaExchange.Web.Controllers
{
    public class MoneyStockController : Controller
    {
        private readonly IAvailableMoneyOnStockRepository _availableMoneyOnStockRepository;

        public MoneyStockController(IAvailableMoneyOnStockRepository availableMoneyOnStockRepository)
        {
            _availableMoneyOnStockRepository = availableMoneyOnStockRepository;
        }

        public IActionResult Index()
        {
            ViewData["activePage"] = "MoneyStock";

            var moneyOnStock = _availableMoneyOnStockRepository.GetAvailableMoneyOnStock()
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

        public IActionResult Deposit(AdminDeposit adminDeposit)
        {
            var depositDetails = new Database.Domain.MoneyOnStock()
            {
                CurrencyName = adminDeposit.Currency,
                Value = adminDeposit.Amount
            };

            _availableMoneyOnStockRepository.AddMoneyToStock(depositDetails);

            var moneyOnStock = _availableMoneyOnStockRepository.GetAvailableMoneyOnStock();
            var currentSaldo = moneyOnStock.SingleOrDefault(a => a.CurrencyName == adminDeposit.Currency);

            ViewData["currentSaldo"] = currentSaldo?.Value ?? 0;
            ViewData["currenncy"] = adminDeposit.Currency;
            ViewData["activePage"] = "MoneyStock";

            return View("DepositCompleted");
        }


    }
}
