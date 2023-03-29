using JediApp.Database.Domain;
using JediApp.Database.Repositories;
using JediApp.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JediApp.Web.Controllers
{
    public class UserSettingsController : Controller
    {
        private readonly IExchangeOfficeBoardService _exchangeOfficeBoardService;

        public UserSettingsController(IExchangeOfficeBoardService exchangeOfficeBoardService)
        {
            _exchangeOfficeBoardService = exchangeOfficeBoardService;
        }

        //public ExchangeOfficeBoardController()
        //{
        //    _exchangeOfficeBoardService = new ExchangeOfficeBoardService(new ExchangeOfficeBoardRepository());
        //}

        // GET: ExchangeOfficeBoardController
        public ActionResult Index()
        {
            ViewData["activePage"] = "UserSettings";

            var currencies = _exchangeOfficeBoardService.GetAllCurrencies();

            return View(currencies);
        }

        // GET: ExchangeOfficeBoardController/Details/5
        public ActionResult Details(Guid id)
        {
            return Ok();

            //var exchangeOffice = _exchangeOfficeService.GetExchangeOfficeById(id);
            //return View(exchangeOffice);
        }

        // GET: ExchangeOfficeBoardController/Edit/5
        public ActionResult Edit(Guid id)
        {
            return Ok();

            //var exchangeOffice = _exchangeOfficeService.GetExchangeOfficeById(id);
            //if (exchangeOffice != null)
            //{
            //    return View(exchangeOffice);
            //}
            //else
            //{
            //    return RedirectToAction(nameof(Index));
            //}

            //return View();
        }

        //// POST: ExchangeOfficeBoardController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        public ActionResult Edit(Guid id, ExchangeOffice exchangeOffice)
        {
            return Ok();

            //try
            //{
            //    return RedirectToAction(nameof(Index));
            //}
            //catch
            //{
            //    return View();
            //}

            //_exchangeOfficeService.UpdateExchangeOffice(id, exchangeOffice);

            //return RedirectToAction(nameof(Index));

            //if (!ModelState.IsValid)
            //{
            //    return View(exchangeOffice);
            //}

            //try
            //{
            //    _exchangeOfficeService.UpdateExchangeOffice(id, exchangeOffice);
            //    return RedirectToAction(nameof(Index));
            //}
            //catch
            //{
            //    return View();
            //}
        }

    }
}

