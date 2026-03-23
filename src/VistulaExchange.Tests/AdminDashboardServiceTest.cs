using Moq;
using VistulaExchange.Database.Domain;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Services.Services.Service;
using Xunit;

namespace VistulaExchange.Tests
{
    public class AdminDashboardServiceTest
    {
        [Fact]
        public async Task GetDashboardAsync_BuildsSummaryFromHistoryAndLiquidity()
        {
            var now = DateTime.Now;
            var transactionHistoryServiceMock = new Mock<ITransactionHistoryService>();
            var userServiceMock = new Mock<IUserService>();
            var availableMoneyOnStockServiceMock = new Mock<IAvailableMoneyOnStockService>();
            var exchangeOfficeBoardServiceMock = new Mock<IExchangeOfficeBoardService>();

            transactionHistoryServiceMock
                .Setup(service => service.GetAllUsersHistoriesAsync())
                .ReturnsAsync(new List<TransactionHistory>
                {
                    new()
                    {
                        CurrencyName = "EUR",
                        Amount = 120m,
                        DateOfTransaction = now.AddDays(-1),
                        Description = "Buy",
                        User = new User { FirstName = "Ada", LastName = "Nowak", Email = "ada@test.pl" }
                    },
                    new()
                    {
                        CurrencyName = "USD",
                        Amount = 90m,
                        DateOfTransaction = now,
                        Description = "Sell",
                        User = new User { FirstName = "Jan", LastName = "Kowalski", Email = "jan@test.pl" }
                    },
                    new()
                    {
                        CurrencyName = "USD",
                        Amount = 50m,
                        DateOfTransaction = now.AddMonths(-1),
                        Description = "Deposit",
                        User = new User { FirstName = "Ewa", LastName = "Test", Email = "ewa@test.pl" }
                    }
                });
            userServiceMock.Setup(service => service.GetUsersCountAsync()).ReturnsAsync(12);
            userServiceMock.Setup(service => service.GetConfirmedUsersCountAsync()).ReturnsAsync(9);
            availableMoneyOnStockServiceMock
                .Setup(service => service.GetAvailableMoneyOnStockAsync())
                .ReturnsAsync(new List<MoneyOnStock>
                {
                    new() { CurrencyName = "PLN", Value = 5000m },
                    new() { CurrencyName = "EUR", Value = 1500m }
                });
            exchangeOfficeBoardServiceMock
                .Setup(service => service.GetAllCurrencies())
                .Returns(new List<Currency>
                {
                    new() { ShortName = "EUR", SellAt = 4.25m, BuyAt = 4.10m },
                    new() { ShortName = "USD", SellAt = 3.95m, BuyAt = 3.80m },
                    new() { ShortName = "PLN", SellAt = 1m, BuyAt = 1m }
                });

            var service = new AdminDashboardService(
                transactionHistoryServiceMock.Object,
                userServiceMock.Object,
                availableMoneyOnStockServiceMock.Object,
                exchangeOfficeBoardServiceMock.Object);

            var result = await service.GetDashboardAsync();

            Assert.Equal(12, result.UserAccounts);
            Assert.Equal(9, result.UserEmailConfirmed);
            Assert.Equal(1, result.DailyTransactions);
            Assert.Equal(2, result.MonthlyTransactions);
            Assert.Equal(3, result.AnnualTransactions);
            Assert.Equal(0.1500m, result.AverageSpread);
            Assert.Equal("USD", result.HottestCurrencyCode);
            Assert.Equal(2, result.HottestCurrencyTransactions);
            Assert.Equal("PLN", result.Liquidity.First().CurrencyCode);
            Assert.Equal("USD", result.Turnovers.First().CurrencyCode);
            Assert.Equal(3, result.LatestTransactions.Count);
        }
    }
}
