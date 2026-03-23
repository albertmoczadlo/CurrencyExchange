using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace VistulaExchange.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IExchangeOfficeBoardService _exchangeOfficeBoardService;

        public HomeController(ILogger<HomeController> logger, IExchangeOfficeBoardService exchangeOfficeBoardService)
        {
            _logger = logger;
            _exchangeOfficeBoardService = exchangeOfficeBoardService;
        }

        public IActionResult Index()
        {
            ViewData["activePage"] = "Home";

            var currencies = _exchangeOfficeBoardService
                .GetAllCurrencies()
                .Where(a => !string.Equals(a.ShortName, "PLN", StringComparison.OrdinalIgnoreCase))
                .Select(CurrencyCardViewModel.FromCurrency)
                .OrderBy(a => a.ShortName)
                .ToList();

            var model = new HomeDashboardViewModel
            {
                Currencies = currencies,
                SpotlightCurrencies = currencies.Take(4).ToList(),
                BestBuyCurrency = currencies.OrderBy(a => a.BuyAt).FirstOrDefault(),
                BestSellCurrency = currencies.OrderByDescending(a => a.SellAt).FirstOrDefault(),
                TightestSpreadCurrency = currencies.OrderBy(a => a.Spread).FirstOrDefault(),
                AverageSpread = currencies.Count > 0 ? Math.Round(currencies.Average(a => a.Spread), 4) : 0m,
                IsAuthenticated = User.Identity?.IsAuthenticated == true,
                IsAdmin = User.IsInRole("Admin"),
                UserDisplayName = User.Identity?.Name ?? string.Empty
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
