using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Services.Services;
using JediApp.Services.Services.Service;
using Microsoft.AspNetCore.Identity.UI.Services;
using Moq;
using Xunit;

namespace Jedi.Tests
{
    public class UserAlarmServiceTest
    {
        private readonly Mock<IUserAlarmsRepository> _userAlarmRepositoryMock;
        private readonly Mock<IEmailSender> _emailSenderMock;

        public UserAlarmServiceTest()
        {
            _userAlarmRepositoryMock = new Mock<IUserAlarmsRepository>();
            _emailSenderMock = new Mock<IEmailSender>();
            _emailSenderMock.Setup(a => a.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public void CheckingIfEmailIsSent_AfterSettingUserAlarms_BuyAt()
        {
            //Arrange
            var currentCurrencyRates = new List<Currency>()
            {
                new Currency()
                {
                    Name = "EUR",
                    ShortName = "EUR",
                    BuyAt = 4.50m,
                    SellAt = 4.70m
                }
            };

            var userAlarmsList = new List<UserAlarm>()
            {
                new UserAlarm()
                {
                    Id = Guid.NewGuid(),
                    CurrencyName = "EUR",
                    ShortName = "EUR",
                    AlarmBuyAt = 4.50m,
                    User = new User() { Email = "test@test.com" }
                }
            };

            _userAlarmRepositoryMock.Setup(a => a.GetUserAlarms()).Returns(userAlarmsList);

            // Act
            var userAlarmService = new UserAlarmsService(_userAlarmRepositoryMock.Object, _emailSenderMock.Object);
            userAlarmService.ExecuteAlarms(currentCurrencyRates);

            //Assert
            _emailSenderMock.Verify(a => a.SendEmailAsync("test@test.com", It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void CheckingIfEmailIsSent_AfterSettingUserAlarms_SellAt()
        {
            //Arrange
            var currentCurrencyRates = new List<Currency>()
            {
                new Currency()
                {
                    Name = "EUR",
                    ShortName = "EUR",
                    SellAt = 4.70m,
                    BuyAt = 4.90m
                }
            };

            var userAlarmsList = new List<UserAlarm>()
            {
                new UserAlarm()
                {
                    Id = Guid.NewGuid(),
                    CurrencyName = "EUR",
                    ShortName = "EUR",
                    AlarmSellAt = 4.70m,
                    User = new User() { Email = "test@test.com" }
                }
            };

            _userAlarmRepositoryMock.Setup(a => a.GetUserAlarms()).Returns(userAlarmsList);

            // Act
            var userAlarmService = new UserAlarmsService(_userAlarmRepositoryMock.Object, _emailSenderMock.Object);
            userAlarmService.ExecuteAlarms(currentCurrencyRates);

            //Assert
            _emailSenderMock.Verify(a => a.SendEmailAsync("test@test.com", It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void CheckingIfEmailIsSent_AfterSettingUserAlarms_NoAlarm()
        {
            //Arrange
            var currentCurrencyRates = new List<Currency>()
            {
                new Currency()
                {
                    Name = "EUR",
                    ShortName = "EUR",
                    SellAt = 4.40m,
                    BuyAt = 4.90m
                }
            };

            var userAlarmsList = new List<UserAlarm>()
            {
                new UserAlarm()
                {
                    Id = Guid.NewGuid(),
                    CurrencyName = "EUR",
                    ShortName = "EUR",
                    AlarmSellAt = 4.50m,
                    AlarmBuyAt = 4.70m,
                    User = new User() { Email = "test@test.com" }
                }
            };

            _userAlarmRepositoryMock.Setup(a => a.GetUserAlarms()).Returns(userAlarmsList);

            // Act
            var userAlarmService = new UserAlarmsService(_userAlarmRepositoryMock.Object, _emailSenderMock.Object);
            userAlarmService.ExecuteAlarms(currentCurrencyRates);

            //Assert
            _emailSenderMock.Verify(a => a.SendEmailAsync("test@test.com", It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }
    }
}
