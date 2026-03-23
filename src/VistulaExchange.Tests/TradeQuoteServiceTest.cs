using Moq;
using VistulaExchange.Database.Domain;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Services.Services.Service;
using Xunit;

namespace VistulaExchange.Tests
{
    public class TradeQuoteServiceTest
    {
        [Fact]
        public async Task BuildQuoteAsync_ForBuy_ClampsRequestedAmountToBalanceAndLiquidity()
        {
            var currencyId = Guid.NewGuid();
            var exchangeOfficeBoardServiceMock = new Mock<IExchangeOfficeBoardService>();
            var userWalletServiceMock = new Mock<IUserWalletService>();
            var availableMoneyOnStockServiceMock = new Mock<IAvailableMoneyOnStockService>();
            var exchangeOfficeServiceMock = new Mock<IExchangeOfficeService>();

            exchangeOfficeBoardServiceMock
                .Setup(service => service.GetAllCurrencies())
                .Returns(new List<Currency>
                {
                    new() { Id = currencyId, Name = "Euro", ShortName = "EUR", BuyAt = 4.00m, SellAt = 3.95m },
                    new() { Id = Guid.NewGuid(), Name = "Polish zloty", ShortName = "PLN", BuyAt = 1m, SellAt = 1m }
                });
            userWalletServiceMock
                .Setup(service => service.GetCurrencyBalanceByCodeAsync("user-1", "PLN"))
                .ReturnsAsync(new WalletPosition { CurrencyAmount = 1000m });
            userWalletServiceMock
                .Setup(service => service.GetCurrencyBalanceByCodeAsync("user-1", "EUR"))
                .ReturnsAsync(new WalletPosition { CurrencyAmount = 12m });
            availableMoneyOnStockServiceMock
                .Setup(service => service.GetAvailableMoneyOnStockAsync())
                .ReturnsAsync(new List<MoneyOnStock>
                {
                    new() { CurrencyName = "EUR", Value = 500m }
                });
            exchangeOfficeServiceMock
                .Setup(service => service.GetAllExchangeOffices())
                .Returns(new List<ExchangeOffice>
                {
                    new() { Markup = 5 }
                });

            var service = new TradeQuoteService(
                exchangeOfficeBoardServiceMock.Object,
                userWalletServiceMock.Object,
                availableMoneyOnStockServiceMock.Object,
                exchangeOfficeServiceMock.Object);

            var quote = await service.BuildQuoteAsync("user-1", currencyId, "Buy", 400m);

            Assert.NotNull(quote);
            Assert.Equal("Buy", quote!.Mode);
            Assert.Equal(248.7562m, quote.RequestedAmount);
            Assert.Equal(995.02m, quote.GrossValue);
            Assert.Equal(4.98m, quote.FeeAmount);
            Assert.Equal(1000.00m, quote.NetValue);
        }

        [Fact]
        public async Task BuildQuoteAsync_ForSell_ComputesNetPayoutAfterFee()
        {
            var currencyId = Guid.NewGuid();
            var exchangeOfficeBoardServiceMock = new Mock<IExchangeOfficeBoardService>();
            var userWalletServiceMock = new Mock<IUserWalletService>();
            var availableMoneyOnStockServiceMock = new Mock<IAvailableMoneyOnStockService>();
            var exchangeOfficeServiceMock = new Mock<IExchangeOfficeService>();

            exchangeOfficeBoardServiceMock
                .Setup(service => service.GetAllCurrencies())
                .Returns(new List<Currency>
                {
                    new() { Id = currencyId, Name = "US Dollar", ShortName = "USD", BuyAt = 4.30m, SellAt = 4.20m },
                    new() { Id = Guid.NewGuid(), Name = "Polish zloty", ShortName = "PLN", BuyAt = 1m, SellAt = 1m }
                });
            userWalletServiceMock
                .Setup(service => service.GetCurrencyBalanceByCodeAsync("user-2", "USD"))
                .ReturnsAsync(new WalletPosition { CurrencyAmount = 100m });
            userWalletServiceMock
                .Setup(service => service.GetCurrencyBalanceByCodeAsync("user-2", "PLN"))
                .ReturnsAsync(new WalletPosition { CurrencyAmount = 200m });
            availableMoneyOnStockServiceMock
                .Setup(service => service.GetAvailableMoneyOnStockAsync())
                .ReturnsAsync(new List<MoneyOnStock>
                {
                    new() { CurrencyName = "PLN", Value = 300m }
                });
            exchangeOfficeServiceMock
                .Setup(service => service.GetAllExchangeOffices())
                .Returns(new List<ExchangeOffice>
                {
                    new() { Markup = 5 }
                });

            var service = new TradeQuoteService(
                exchangeOfficeBoardServiceMock.Object,
                userWalletServiceMock.Object,
                availableMoneyOnStockServiceMock.Object,
                exchangeOfficeServiceMock.Object);

            var quote = await service.BuildQuoteAsync("user-2", currencyId, "Sell", 10m);

            Assert.NotNull(quote);
            Assert.Equal("Sell", quote!.Mode);
            Assert.Equal(10m, quote.RequestedAmount);
            Assert.Equal(42.00m, quote.GrossValue);
            Assert.Equal(0.21m, quote.FeeAmount);
            Assert.Equal(41.79m, quote.NetValue);
        }
    }
}
