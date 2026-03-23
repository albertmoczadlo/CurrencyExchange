using VistulaExchange.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using UserAlarm = VistulaExchange.Database.Domain.UserAlarm;

namespace VistulaExchange.Web.Controllers
{
    public class UserAlarmController : Controller
    {
        private readonly IExchangeOfficeBoardService _exchangeOfficeBoardService;
        private readonly IUserAlarmsService _userAlarmService;

        public UserAlarmController(IExchangeOfficeBoardService exchangeOfficeBoardService, IUserAlarmsService userAlarmService)
        {
            _exchangeOfficeBoardService = exchangeOfficeBoardService;
            _userAlarmService = userAlarmService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["activePage"] = "UserAlarm";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var userAlarms = await _userAlarmService.GetUserAlarmsAsync(userId);

            return View(userAlarms);
        }

        public IActionResult Create()
        {
            ViewData["activePage"] = "UserAlarm";
            PopulateAlarmEditorState();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAlarm(UserAlarm userAlarm)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            userAlarm.UserId = userId;
            userAlarm.CurrencyName = userAlarm.ShortName;

            await _userAlarmService.AddUserAlarmAsync(userAlarm);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewData["activePage"] = "UserAlarm";
            PopulateAlarmEditorState();

            var userAlarm = await _userAlarmService.GetUserAlarmAsync(id);
            return View(userAlarm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(UserAlarm userAlarm)
        {
            userAlarm.CurrencyName = userAlarm.ShortName;

            await _userAlarmService.EditUserAlarmAsync(userAlarm);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _userAlarmService.DeleteUserAlarmAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private void PopulateAlarmEditorState()
        {
            var currencies = _exchangeOfficeBoardService.GetAllCurrencies()
                .Where(currency => !string.Equals(currency.ShortName, "PLN", StringComparison.OrdinalIgnoreCase))
                .OrderBy(currency => currency.ShortName)
                .ToList();

            ViewData["currencies"] = currencies.Select(currency => currency.ShortName);
            ViewData["currencyRates"] = JsonSerializer.Serialize(currencies.Select(currency => new
            {
                code = currency.ShortName,
                buy = currency.BuyAt,
                sell = currency.SellAt,
                name = currency.Name
            }));
        }
    }
}
