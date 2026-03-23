using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using VistulaExchange.Services.Models;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Web.Controllers;
using Xunit;

namespace VistulaExchange.Tests
{
    public class UserAccountBalanceControllerTest
    {
        [Fact]
        public async Task Index_ReturnsChallenge_WhenUserIsMissing()
        {
            var dashboardServiceMock = new Mock<IUserDashboardService>();
            var controller = new UserAccountBalance(dashboardServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Index();

            Assert.IsType<ChallengeResult>(result);
        }

        [Fact]
        public async Task Index_ReturnsViewWithDashboard_WhenUserExists()
        {
            var dashboardServiceMock = new Mock<IUserDashboardService>();
            dashboardServiceMock
                .Setup(service => service.GetDashboardAsync("user-1"))
                .ReturnsAsync(new UserDashboardDto { UserDisplayName = "Anna" });
            var controller = new UserAccountBalance(dashboardServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = BuildHttpContext("user-1")
            };

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<UserDashboardDto>(viewResult.Model);
            Assert.Equal("Anna", model.UserDisplayName);
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
