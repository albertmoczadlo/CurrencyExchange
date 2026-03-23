using Moq;
using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Models;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Services.Services.Service;
using Xunit;

namespace VistulaExchange.Tests
{
    public class UserDashboardServiceTest
    {
        [Fact]
        public async Task GetDashboardAsync_ComputesPortfolioMetricsAndInsights()
        {
            var now = DateTime.Now;
            var userWalletServiceMock = new Mock<IUserWalletService>();
            var transactionHistoryServiceMock = new Mock<ITransactionHistoryService>();
            var userAlarmsServiceMock = new Mock<IUserAlarmsService>();
            var userServiceMock = new Mock<IUserService>();
            var exchangeOfficeBoardServiceMock = new Mock<IExchangeOfficeBoardService>();
            var nbpJsonServiceMock = new Mock<INbpJsonService>();

            userServiceMock
                .Setup(service => service.GetUserByIdAsync("user-1"))
                .ReturnsAsync(new User { FirstName = "Anna", LastName = "Nowak", Email = "anna@test.pl" });
            userWalletServiceMock
                .Setup(service => service.GetWalletAsync("user-1"))
                .ReturnsAsync(new Wallet
                {
                    WalletPositions = new List<WalletPosition>
                    {
                        new()
                        {
                            CurrencyAmount = 10m,
                            Currency = new Currency { ShortName = "EUR", Name = "Euro" }
                        },
                        new()
                        {
                            CurrencyAmount = 100m,
                            Currency = new Currency { ShortName = "PLN", Name = "Polish zloty" }
                        }
                    }
                });
            transactionHistoryServiceMock
                .Setup(service => service.GetUserHistoryByUserIdAsync("user-1"))
                .ReturnsAsync(new List<TransactionHistory>
                {
                    new()
                    {
                        CurrencyName = "EUR",
                        Amount = 10m,
                        DateOfTransaction = now,
                        Description = "Buy",
                        User = new User { FirstName = "Anna", LastName = "Nowak", Email = "anna@test.pl" }
                    }
                });
            userAlarmsServiceMock
                .Setup(service => service.GetUserAlarmsAsync("user-1"))
                .ReturnsAsync(new List<UserAlarm>
                {
                    new() { ShortName = "EUR", AlarmSellAt = 4.50m }
                });
            exchangeOfficeBoardServiceMock
                .Setup(service => service.GetAllCurrencies())
                .Returns(new List<Currency>
                {
                    new() { Id = Guid.NewGuid(), ShortName = "EUR", Name = "Euro", Country = "European Union", BuyAt = 4.30m, SellAt = 4.20m },
                    new() { Id = Guid.NewGuid(), ShortName = "PLN", Name = "Polish zloty", Country = "Poland", BuyAt = 1m, SellAt = 1m }
                });
            nbpJsonServiceMock
                .Setup(service => service.GetCurrencyHistoryAsync("EUR", 2))
                .ReturnsAsync(new List<CurrencyHistoryPoint>
                {
                    new() { Date = now.AddDays(-1), BuyAt = 4.05m, SellAt = 3.90m },
                    new() { Date = now, BuyAt = 4.15m, SellAt = 4.00m }
                });

            var service = new UserDashboardService(
                userWalletServiceMock.Object,
                transactionHistoryServiceMock.Object,
                userAlarmsServiceMock.Object,
                userServiceMock.Object,
                exchangeOfficeBoardServiceMock.Object,
                nbpJsonServiceMock.Object);

            var result = await service.GetDashboardAsync("user-1");

            Assert.Equal("Anna Nowak", result.UserDisplayName);
            Assert.Equal(142.00m, result.TotalValueInPln);
            Assert.Equal(3.00m, result.DailyChangeValue);
            Assert.Equal(2.16m, result.DailyChangePercent);
            Assert.Equal("EUR", result.DefaultChartCurrencyCode);
            Assert.Equal(2, result.Positions.Count);
            Assert.Single(result.ActiveAlarms);
            Assert.NotEmpty(result.Insights);
            Assert.Single(result.RecentTransactions);
            Assert.Single(result.MarketQuotes);
        }
    }
}
