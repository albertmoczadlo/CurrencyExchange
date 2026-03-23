using Microsoft.AspNetCore.Mvc;
using Moq;
using VistulaExchange.Services.Models;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Web.Controllers;
using Xunit;

namespace VistulaExchange.Tests
{
    public class AdminReportControllerTest
    {
        [Fact]
        public async Task Index_ReturnsViewWithDashboardModel()
        {
            var dashboardServiceMock = new Mock<IAdminDashboardService>();
            dashboardServiceMock
                .Setup(service => service.GetDashboardAsync())
                .ReturnsAsync(new AdminDashboardDto { UserAccounts = 4 });
            var controller = new AdminReportController(dashboardServiceMock.Object);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AdminDashboardDto>(viewResult.Model);
            Assert.Equal(4, model.UserAccounts);
        }
    }
}
