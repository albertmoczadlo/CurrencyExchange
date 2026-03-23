using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace VistulaExchange.Services.Services.Service
{
    public class UserAlarmsService : IUserAlarmsService
    {
        private readonly IUserAlarmsRepository _userAlarmRepository;
        private readonly IEmailSender _emailSender;
        private const string EmailTemplate = "<html>\r\n<body>\r\n<h3>Dear User</h3>\r\n<p>There has been alarm that you setup for currency [CURRENCY]</p><p>Current rate is in range of your alarm: [CURRENCY_RATE]</p>\r\n</body>\r\n</html>";

        public UserAlarmsService(IUserAlarmsRepository userAlarmRepository, IEmailSender emailSender)
        {
            _userAlarmRepository = userAlarmRepository;
            _emailSender = emailSender;
        }

        public async Task ExecuteAlarmsAsync(List<Currency> currencies)
        {
            var userAlarms = await _userAlarmRepository.GetUserAlarmsAsync();

            foreach (var userAlarm in userAlarms)
            {
                var currency = currencies.FirstOrDefault(a =>
                    string.Equals(a.ShortName, userAlarm.ShortName, StringComparison.OrdinalIgnoreCase));

                if (currency is null || string.IsNullOrWhiteSpace(userAlarm.User?.Email))
                {
                    continue;
                }

                if (userAlarm.AlarmBuyAt > 0 && currency.BuyAt <= userAlarm.AlarmBuyAt)
                {
                    await SendAlarmEmailAsync(userAlarm.User.Email, currency.ShortName, $"BUY AT: {currency.BuyAt}");
                }

                if (userAlarm.AlarmSellAt > 0 && currency.SellAt >= userAlarm.AlarmSellAt)
                {
                    await SendAlarmEmailAsync(userAlarm.User.Email, currency.ShortName, $"SELL AT: {currency.SellAt}");
                }
            }
        }

        private Task SendAlarmEmailAsync(string email, string currencyCode, string currentRate)
        {
            var emailContent = EmailTemplate
                .Replace("[CURRENCY]", currencyCode)
                .Replace("[CURRENCY_RATE]", currentRate);

            return _emailSender.SendEmailAsync(email, "Currency Exchange Alarm", emailContent);
        }
    }
}
