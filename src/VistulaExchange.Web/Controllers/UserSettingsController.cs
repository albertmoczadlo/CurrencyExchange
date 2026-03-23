using VistulaExchange.Database.Domain;
using VistulaExchange.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace VistulaExchange.Web.Controllers
{
    public class UserSettingsController : Controller
    {
        private readonly IExchangeOfficeBoardService _exchangeOfficeBoardService;

        public UserSettingsController(IExchangeOfficeBoardService exchangeOfficeBoardService)
        {
            _exchangeOfficeBoardService = exchangeOfficeBoardService;
        }

        public ActionResult Index()
        {
            ViewData["activePage"] = "UserSettings";

            var currencies = _exchangeOfficeBoardService.GetAllCurrencies();

            return View(currencies);
        }

        public ActionResult Details(Guid id)
        {
            return Ok();
        }

        public ActionResult Edit(Guid id)
        {
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, ExchangeOffice exchangeOffice)
        {
            return Ok();
        }

    }
}

