using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserAlarm = VistulaExchange.Database.Domain.UserAlarm;

namespace VistulaExchange.Web.Controllers
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

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userAlarms = await _userAlarmRepository.GetUserAlarmsAsync(userId);

            return View(userAlarms);
        }

        public IActionResult Create()
        {
            ViewData["currencies"] = _exchangeOfficeBoardService.GetAllCurrencies().Select(a => a.ShortName);

            return View();
        }

        public async Task<IActionResult> CreateAlarm(UserAlarm userAlarm)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            userAlarm.UserId = userId;
            userAlarm.CurrencyName = userAlarm.ShortName;

            await _userAlarmRepository.AddUserAlarmAsync(userAlarm);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewData["currencies"] = _exchangeOfficeBoardService.GetAllCurrencies().Select(a => a.ShortName);

            var userAlarm = await _userAlarmRepository.GetUserAlarmAsync(id);
            return View(userAlarm);
        }

        public async Task<IActionResult> EditConfirmed(UserAlarm userAlarm)
        {
            userAlarm.CurrencyName = userAlarm.ShortName;

            await _userAlarmRepository.EditUserAlarmAsync(userAlarm);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _userAlarmRepository.DeleteUserAlarmAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
