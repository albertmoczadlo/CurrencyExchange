using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Services.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace JediApp.Services.Services.Service
{
    public class UserAlarmsService : IUserAlarmsService
    {
        private readonly IUserAlarmsRepository _userAlarmRepository;
        private readonly IEmailSender _emailSender;
        private string EmailContent = "<html>\r\n<body>\r\n<h3>Dear User</h3>\r\n<p>There has been alarm that you setup for currency [CURRENCY]</p><p>Current rate is in range of your alarm: [CURRENCY_RATE]</p>\r\n</body>\r\n</html>";

        public UserAlarmsService(IUserAlarmsRepository userAlarmRepository, IEmailSender emailSender)
        {
            _userAlarmRepository = userAlarmRepository;
            _emailSender = emailSender;
        }

        public void ExecuteAlarms(List<Currency> currencies)
        {
            var userAlarms = _userAlarmRepository.GetUserAlarms();

            foreach (var userAlarm in userAlarms)
            {
                var currency = currencies.Where(a => a.ShortName == userAlarm.ShortName).FirstOrDefault();

                if (userAlarm.AlarmBuyAt > 0 && currency.BuyAt <= userAlarm.AlarmBuyAt)
                {
                    // EXECUTE ALARM
                    EmailContent = EmailContent.Replace("[CURRENCY]", currency.ShortName);
                    EmailContent = EmailContent.Replace("[CURRENCY_RATE]", "BUY AT: " + currency.BuyAt);
                    _emailSender.SendEmailAsync(userAlarm.User.Email, "Currency Exchange Alarm", EmailContent);
                }

                if (userAlarm.AlarmSellAt > 0 && currency.SellAt >= userAlarm.AlarmSellAt)
                {
                    // EXECUTE ALARM
                    EmailContent = EmailContent.Replace("[CURRENCY]", currency.ShortName);
                    EmailContent = EmailContent.Replace("[CURRENCY_RATE]", "SELL AT: " + currency.BuyAt);
                    _emailSender.SendEmailAsync(userAlarm.User.Email, "Currency Exchange Alarm", EmailContent);
                }
            }
        }
    }
}
