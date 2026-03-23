using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Service;
using Microsoft.AspNetCore.Identity.UI.Services;
using Moq;
using Xunit;

namespace VistulaExchange.Tests
{
    public class UserAlarmServiceTest
    {
        private readonly Mock<IUserAlarmsRepository> _userAlarmRepositoryMock;
        private readonly Mock<IEmailSender> _emailSenderMock;

        public UserAlarmServiceTest()
        {
            _userAlarmRepositoryMock = new Mock<IUserAlarmsRepository>();
            _emailSenderMock = new Mock<IEmailSender>();
            _emailSenderMock
                .Setup(a => a.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task CheckingIfEmailIsSent_AfterSettingUserAlarms_BuyAt()
        {
            var currentCurrencyRates = new List<Currency>
            {
                new Currency
                {
                    Name = "EUR",
                    ShortName = "EUR",
                    BuyAt = 4.50m,
                    SellAt = 4.70m
                }
            };

            var userAlarmsList = new List<UserAlarm>
            {
                new UserAlarm
                {
                    Id = Guid.NewGuid(),
                    CurrencyName = "EUR",
                    ShortName = "EUR",
                    AlarmBuyAt = 4.50m,
                    User = new User { Email = "test@test.com" }
                }
            };

            _userAlarmRepositoryMock.Setup(a => a.GetUserAlarmsAsync()).ReturnsAsync(userAlarmsList);

            var userAlarmService = new UserAlarmsService(_userAlarmRepositoryMock.Object, _emailSenderMock.Object);
            await userAlarmService.ExecuteAlarmsAsync(currentCurrencyRates);

            _emailSenderMock.Verify(a => a.SendEmailAsync("test@test.com", It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task CheckingIfEmailIsSent_AfterSettingUserAlarms_SellAt()
        {
            var currentCurrencyRates = new List<Currency>
            {
                new Currency
                {
                    Name = "EUR",
                    ShortName = "EUR",
                    SellAt = 4.70m,
                    BuyAt = 4.90m
                }
            };

            var userAlarmsList = new List<UserAlarm>
            {
                new UserAlarm
                {
                    Id = Guid.NewGuid(),
                    CurrencyName = "EUR",
                    ShortName = "EUR",
                    AlarmSellAt = 4.70m,
                    User = new User { Email = "test@test.com" }
                }
            };

            _userAlarmRepositoryMock.Setup(a => a.GetUserAlarmsAsync()).ReturnsAsync(userAlarmsList);

            var userAlarmService = new UserAlarmsService(_userAlarmRepositoryMock.Object, _emailSenderMock.Object);
            await userAlarmService.ExecuteAlarmsAsync(currentCurrencyRates);

            _emailSenderMock.Verify(a => a.SendEmailAsync("test@test.com", It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task CheckingIfEmailIsSent_AfterSettingUserAlarms_NoAlarm()
        {
            var currentCurrencyRates = new List<Currency>
            {
                new Currency
                {
                    Name = "EUR",
                    ShortName = "EUR",
                    SellAt = 4.40m,
                    BuyAt = 4.90m
                }
            };

            var userAlarmsList = new List<UserAlarm>
            {
                new UserAlarm
                {
                    Id = Guid.NewGuid(),
                    CurrencyName = "EUR",
                    ShortName = "EUR",
                    AlarmSellAt = 4.50m,
                    AlarmBuyAt = 4.70m,
                    User = new User { Email = "test@test.com" }
                }
            };

            _userAlarmRepositoryMock.Setup(a => a.GetUserAlarmsAsync()).ReturnsAsync(userAlarmsList);

            var userAlarmService = new UserAlarmsService(_userAlarmRepositoryMock.Object, _emailSenderMock.Object);
            await userAlarmService.ExecuteAlarmsAsync(currentCurrencyRates);

            _emailSenderMock.Verify(a => a.SendEmailAsync("test@test.com", It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async Task CheckingIfEmailIsNotSent_WhenCurrencyDoesNotExistInCurrentRates()
        {
            var currentCurrencyRates = new List<Currency>
            {
                new Currency
                {
                    Name = "USD",
                    ShortName = "USD",
                    BuyAt = 4.50m,
                    SellAt = 4.70m
                }
            };

            var userAlarmsList = new List<UserAlarm>
            {
                new UserAlarm
                {
                    Id = Guid.NewGuid(),
                    CurrencyName = "EUR",
                    ShortName = "EUR",
                    AlarmBuyAt = 4.50m,
                    User = new User { Email = "test@test.com" }
                }
            };

            _userAlarmRepositoryMock.Setup(a => a.GetUserAlarmsAsync()).ReturnsAsync(userAlarmsList);

            var userAlarmService = new UserAlarmsService(_userAlarmRepositoryMock.Object, _emailSenderMock.Object);
            await userAlarmService.ExecuteAlarmsAsync(currentCurrencyRates);

            _emailSenderMock.Verify(a => a.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }
    }
}
