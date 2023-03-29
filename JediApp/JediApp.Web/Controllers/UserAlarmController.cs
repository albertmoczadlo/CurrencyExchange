using JediApp.Database.Interface;
using JediApp.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserAlarm = JediApp.Database.Domain.UserAlarm;

namespace JediApp.Web.Controllers
{
    public class UserAlarmController : Controller
    {
        private readonly IExchangeOfficeBoardService _exchangeOfficeBoardService;
        private readonly IUserAlarmsRepository _userAlarmRepository;

        public UserAlarmController(IExchangeOfficeBoardService exchangeOfficeBoardService, IUserAlarmsRepository userAlarmRepository)
        {
            _exchangeOfficeBoardService = exchangeOfficeBoardService;
            _userAlarmRepository = userAlarmRepository;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userAlarms = _userAlarmRepository.GetUserAlarms(userId);

            return View(userAlarms);
        }

        public IActionResult Create()
        {
            ViewData["currencies"] = _exchangeOfficeBoardService.GetAllCurrencies().Select(a => a.ShortName );

            return View();
        }

        public IActionResult CreateAlarm(UserAlarm userAlarm)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            userAlarm.UserId = userId;
            userAlarm.CurrencyName = userAlarm.ShortName;

            _userAlarmRepository.AddUserAlarm(userAlarm);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(string id)
        {
            ViewData["currencies"] = _exchangeOfficeBoardService.GetAllCurrencies().Select(a => a.ShortName);

            var userAlarm = _userAlarmRepository.GetUserAlarm(id);
            return View(userAlarm);
        }

        public IActionResult EditConfirmed(UserAlarm userAlarm)
        {
            userAlarm.CurrencyName = userAlarm.ShortName;

            _userAlarmRepository.EditUserAlarm(userAlarm);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(string id)
        {
            _userAlarmRepository.DeleteUserAlarm(id);

            return RedirectToAction("Index");
        }
    }
}
