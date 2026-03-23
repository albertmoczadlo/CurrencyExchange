using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Models;
using VistulaExchange.Services.Models;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Web.Controllers;
using VistulaExchange.Web.Models;
using Xunit;

namespace VistulaExchange.Tests
{
    public class UserExchangeOfficeBoardControllerTest
    {
        [Fact]
        public async Task Buy_ReturnsChallenge_WhenUserIsMissing()
        {
            var controller = CreateController();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Buy(Guid.NewGuid());

            Assert.IsType<ChallengeResult>(result);
        }

        [Fact]
        public async Task Buy_ReturnsView_WhenQuoteExists()
        {
            var tradeQuoteServiceMock = new Mock<ITradeQuoteService>();
            tradeQuoteServiceMock
                .Setup(service => service.BuildQuoteAsync("user-1", It.IsAny<Guid>(), "Buy", null))
                .ReturnsAsync(new TradeQuoteDto
                {
                    CurrencyId = Guid.NewGuid(),
                    CurrencyName = "Euro",
                    Mode = "Buy",
                    SourceCurrencyCode = "PLN",
                    TargetCurrencyCode = "EUR",
                    MaxAmount = 10m,
                    QuoteLockedUntilUtc = DateTime.UtcNow.AddMinutes(1)
                });
            var controller = CreateController(tradeQuoteService: tradeQuoteServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = BuildHttpContext("user-1")
            };

            var result = await controller.Buy(Guid.NewGuid());

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<QuickExchangeViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task BuyPost_ReturnsBuyViewWithError_WhenQuoteExpired()
        {
            var tradeQuoteServiceMock = new Mock<ITradeQuoteService>();
            tradeQuoteServiceMock
                .Setup(service => service.BuildQuoteAsync("user-1", It.IsAny<Guid>(), "Buy", 2m))
                .ReturnsAsync(new TradeQuoteDto
                {
                    CurrencyId = Guid.NewGuid(),
                    CurrencyName = "Euro",
                    Mode = "Buy",
                    SourceCurrencyCode = "PLN",
                    TargetCurrencyCode = "EUR",
                    RequestedAmount = 2m,
                    MaxAmount = 5m,
                    QuoteLockedUntilUtc = DateTime.UtcNow.AddMinutes(1)
                });
            var controller = CreateController(tradeQuoteService: tradeQuoteServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = BuildHttpContext("user-1")
            };

            var result = await controller.Buy(new QuickExchangeViewModel
            {
                CurrencyId = Guid.NewGuid(),
                RequestedAmount = 2m,
                MaxAmount = 5m,
                QuoteLockedUntilUtc = DateTime.UtcNow.AddSeconds(-5)
            });

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Buy", viewResult.ViewName);
            var model = Assert.IsType<QuickExchangeViewModel>(viewResult.Model);
            Assert.Contains("expired", model.ErrorMessage, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task BuyPost_UpdatesWalletAndStock_WhenQuoteIsValid()
        {
            var availableMoneyOnStockServiceMock = new Mock<IAvailableMoneyOnStockService>();
            var userWalletServiceMock = new Mock<IUserWalletService>();
            availableMoneyOnStockServiceMock
                .Setup(service => service.SubtractMoneyFromStockAsync(It.IsAny<MoneyOnStock>()))
                .ReturnsAsync(string.Empty);
            userWalletServiceMock
                .Setup(service => service.GetCurrencyBalanceByCodeAsync("user-1", "PLN"))
                .ReturnsAsync(new WalletPosition { CurrencyAmount = 800m });

            var controller = CreateController(
                userWalletService: userWalletServiceMock.Object,
                availableMoneyOnStockService: availableMoneyOnStockServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = BuildHttpContext("user-1")
            };

            var result = await controller.Buy(new QuickExchangeViewModel
            {
                CurrencyId = Guid.NewGuid(),
                CurrencyName = "Euro",
                Mode = "Buy",
                SourceCurrencyCode = "PLN",
                TargetCurrencyCode = "EUR",
                RequestedAmount = 2m,
                MaxAmount = 5m,
                NetValue = 8.20m,
                QuoteLockedUntilUtc = DateTime.UtcNow.AddMinutes(1)
            });

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("BuyCompleted", viewResult.ViewName);
            availableMoneyOnStockServiceMock.Verify(service => service.AddMoneyToStockAsync(It.Is<MoneyOnStock>(stock =>
                stock.CurrencyName == "PLN" && stock.Value == 8.20m)), Times.Once);
            userWalletServiceMock.Verify(service => service.WithdrawalAsync("user-1", "PLN", 8.20m, "Buy"), Times.Once);
            userWalletServiceMock.Verify(service => service.DepositAsync("user-1", "EUR", 2m, "Buy"), Times.Once);
        }

        [Fact]
        public async Task History_ReturnsBadRequest_WhenCurrencyCodeIsMissing()
        {
            var controller = CreateController();

            var result = await controller.History(string.Empty);

            Assert.IsType<BadRequestResult>(result);
        }

        private static UserExchangeOfficeBoardController CreateController(
            IExchangeOfficeBoardService? exchangeOfficeBoardService = null,
            IUserWalletService? userWalletService = null,
            IAvailableMoneyOnStockService? availableMoneyOnStockService = null,
            ITradeQuoteService? tradeQuoteService = null,
            INbpJsonService? nbpJsonService = null)
        {
            return new UserExchangeOfficeBoardController(
                exchangeOfficeBoardService ?? Mock.Of<IExchangeOfficeBoardService>(service =>
                    service.GetAllCurrencies() == new List<Currency>
                    {
                        new() { Id = Guid.NewGuid(), ShortName = "EUR", Name = "Euro", Country = "EU", BuyAt = 4.1m, SellAt = 4.0m },
                        new() { Id = Guid.NewGuid(), ShortName = "PLN", Name = "Polish zloty", Country = "PL", BuyAt = 1m, SellAt = 1m }
                    }),
                userWalletService ?? Mock.Of<IUserWalletService>(),
                availableMoneyOnStockService ?? Mock.Of<IAvailableMoneyOnStockService>(),
                tradeQuoteService ?? Mock.Of<ITradeQuoteService>(),
                nbpJsonService ?? Mock.Of<INbpJsonService>(service =>
                    service.GetCurrencyHistoryAsync("EUR", 30) == Task.FromResult<IReadOnlyList<CurrencyHistoryPoint>>(new List<CurrencyHistoryPoint>())));
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
