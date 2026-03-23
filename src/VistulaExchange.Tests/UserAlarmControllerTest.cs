using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using VistulaExchange.Database.Domain;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Web.Controllers;
using Xunit;

namespace VistulaExchange.Tests
{
    public class UserAlarmControllerTest
    {
        [Fact]
        public async Task Index_ReturnsChallenge_WhenUserIsMissing()
        {
            var exchangeOfficeBoardServiceMock = new Mock<IExchangeOfficeBoardService>();
            var userAlarmServiceMock = new Mock<IUserAlarmsService>();
            var controller = new UserAlarmController(exchangeOfficeBoardServiceMock.Object, userAlarmServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Index();

            Assert.IsType<ChallengeResult>(result);
        }

        [Fact]
        public async Task CreateAlarm_AssignsUserIdAndCurrencyNameBeforeSaving()
        {
            var exchangeOfficeBoardServiceMock = new Mock<IExchangeOfficeBoardService>();
            var userAlarmServiceMock = new Mock<IUserAlarmsService>();
            var controller = new UserAlarmController(exchangeOfficeBoardServiceMock.Object, userAlarmServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = BuildHttpContext("user-1")
            };

            var alarm = new UserAlarm
            {
                ShortName = "EUR",
                AlarmBuyAt = 4.20m
            };

            var result = await controller.CreateAlarm(alarm);

            Assert.IsType<RedirectToActionResult>(result);
            userAlarmServiceMock.Verify(service => service.AddUserAlarmAsync(It.Is<UserAlarm>(saved =>
                saved.UserId == "user-1" && saved.CurrencyName == "EUR")), Times.Once);
        }

        private static DefaultHttpContext BuildHttpContext(string userId)
        {
            var context = new DefaultHttpContext();
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "TestAuth"));
            return context;
        }
    }
}
