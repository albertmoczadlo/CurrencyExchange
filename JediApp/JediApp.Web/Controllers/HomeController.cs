using JediApp.Services.Services.Interfaces;
using JediApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JediApp.Web.Controllers
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
            var allCurrencies = _exchangeOfficeBoardService.GetAllCurrencies();

            var euro = allCurrencies.Where(a => a.ShortName == "EUR").FirstOrDefault();
            if(euro != null)
            {
                ViewData["EUR_sell"] = euro.SellAt;
                ViewData["EUR_buy"] = euro.BuyAt;
            }

            var dolar = allCurrencies.Where(a => a.ShortName == "USD").FirstOrDefault();
            if(dolar != null)
            {
                ViewData["USD_sell"] = dolar.SellAt;
                ViewData["USD_buy"] = dolar.BuyAt;
            }

            var frank = allCurrencies.Where(a => a.ShortName == "CHF").FirstOrDefault();
            if(frank != null)
            {
                ViewData["CHF_sell"] = frank.SellAt;
                ViewData["CHF_buy"] = frank.BuyAt;
            }

            var funt = allCurrencies.Where(a => a.ShortName == "GBP").FirstOrDefault();
            if(funt != null)
            {
                ViewData["GBP_sell"] = funt.SellAt;
                ViewData["GBP_buy"] = funt.BuyAt;
            }

            return View();
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