﻿using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Database.Repositories;
using JediApp.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JediApp.Web.Controllers
{
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
        public ActionResult Index()
        {
            ViewData["activePage"] = "AdminExchange";

            var model = _exchangeOfficeBoardService.GetAllCurrencies();

            var pln = model.Where(m => m.ShortName.ToLower().Equals("pln")).FirstOrDefault();
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Currency newCurrency)
        {
            _exchangeOfficeBoardService.AddCurrency(newCurrency);

            return RedirectToAction(nameof(Index));

            if (!ModelState.IsValid)
            {
                return View(newCurrency);
            }
            try
            {
                _exchangeOfficeBoardService.AddCurrency(newCurrency);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
            _exchangeOfficeBoardService.UpdateCurrency(id, currency);

            return RedirectToAction(nameof(Index));

            if (!ModelState.IsValid)
            {
                return View(currency);
            }

            try
            {
                _exchangeOfficeBoardService.UpdateCurrency(id, currency);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
            try
            {
                var currencyToDelete = _exchangeOfficeBoardService.GetCurrencyById(id);
                _exchangeOfficeBoardService.DeleteCurrencyByShortName(currencyToDelete.ShortName);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCurrencyFromNbpApi()
        {

            List<Currency> nbpCurrencies = _nbpJsonService.GetAllCurrencies();

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
        public ActionResult UpdateCurrencyRatesFromNbpApi()
        {

            List<Currency> nbpCurrencies = _nbpJsonService.GetAllCurrencies();
            var currencies = _exchangeOfficeBoardService.GetAllCurrencies();

            foreach (var currency in currencies)
            {
                try
                {
                    if (!currency.ShortName.ToLower().Equals("pln"))
                    {
                        var currencyToUpdate = nbpCurrencies.Where(nc => nc.ShortName.ToLower().Equals(currency.ShortName.ToLower())).FirstOrDefault();
                        if (currencyToUpdate != null)
                        {
                            currency.BuyAt = currencyToUpdate.BuyAt;
                            currency.SellAt = currencyToUpdate.SellAt;
                            _exchangeOfficeBoardService.UpdateCurrency(currency.Id, currency);
                        }
                    }

                }
                catch
                {
                    //pln
                }

            }

            _userAlarmService.ExecuteAlarms(currencies);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Markup()
        {

            var currencies = _exchangeOfficeBoardService.GetAllCurrencies();

            foreach (var currency in currencies)
            {

                var markup = _exchangeOfficeService.GetAllExchangeOffices().FirstOrDefault().Markup;
                currency.BuyAt *= 1 + (decimal)markup / 200;
                currency.SellAt *= 1 - (decimal)markup / 200;
                _exchangeOfficeBoardService.UpdateCurrency(currency.Id, currency);

            }

            return RedirectToAction(nameof(Index));
        }

    }
}
