using JediApp.Database.Domain;
using JediApp.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JediApp.Web.Controllers
{
    public class AdminExchangeOfficeSettingsController : Controller
    {
        private readonly IExchangeOfficeService _exchangeOfficeService;

        public AdminExchangeOfficeSettingsController(IExchangeOfficeService exchangeOfficeService)
        {
            _exchangeOfficeService = exchangeOfficeService;
        }
        public ActionResult Index()
        {
            ViewData["activePage"] = "AdminExchangeOfficeSettings";

            var model = _exchangeOfficeService.GetAllExchangeOffices();

            return View(model);
        }

        public ActionResult Details(Guid id)
        {
            var exchangeOffice = _exchangeOfficeService.GetExchangeOfficeById(id);
            return View(exchangeOffice);
        }

        public ActionResult Edit(Guid id)
        {
            var exchangeOffice = _exchangeOfficeService.GetExchangeOfficeById(id);
            if (exchangeOffice != null)
            {
                return View(exchangeOffice);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, ExchangeOffice exchangeOffice)
        {
            _exchangeOfficeService.UpdateExchangeOffice(id, exchangeOffice);

            return RedirectToAction(nameof(Index));

            if (!ModelState.IsValid)
            {
                return View(exchangeOffice);
            }

            try
            {
                _exchangeOfficeService.UpdateExchangeOffice(id, exchangeOffice);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
