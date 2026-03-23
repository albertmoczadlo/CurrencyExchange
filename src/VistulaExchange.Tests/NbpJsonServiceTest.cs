using Moq;
using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Database.Models;
using VistulaExchange.Services.Services.Service;
using Xunit;

namespace VistulaExchange.Tests
{
    public class NbpJsonServiceTest
    {
        [Fact]
        public async Task GetAllCurrenciesAsync_ReturnsRepositoryData()
        {
            var expected = new List<Currency>
            {
                new() { ShortName = "EUR", Name = "Euro" }
            };
            var repositoryMock = new Mock<INbpJsonRepository>();
            repositoryMock
                .Setup(repository => repository.GetAllCurrenciesAsync())
                .ReturnsAsync(expected);

            var service = new NbpJsonService(repositoryMock.Object);

            var result = await service.GetAllCurrenciesAsync();

            Assert.Same(expected, result);
        }

        [Fact]
        public async Task GetNBPJsonTableInfoAsync_ReturnsRepositorySnapshot()
        {
            var expected = new NBPJsonRoot();
            var repositoryMock = new Mock<INbpJsonRepository>();
            repositoryMock
                .Setup(repository => repository.GetNBPJsonTableInfoAsync())
                .ReturnsAsync(expected);

            var service = new NbpJsonService(repositoryMock.Object);

            var result = await service.GetNBPJsonTableInfoAsync();

            Assert.Same(expected, result);
        }

        [Fact]
        public async Task BrowseCurrencyAsync_ReturnsFilteredRepositoryData()
        {
            var expected = new List<Currency>
            {
                new() { ShortName = "USD", Name = "US Dollar" }
            };
            var repositoryMock = new Mock<INbpJsonRepository>();
            repositoryMock
                .Setup(repository => repository.BrowseCurrencyAsync("usd"))
                .ReturnsAsync(expected);

            var service = new NbpJsonService(repositoryMock.Object);

            var result = await service.BrowseCurrencyAsync("usd");

            Assert.Same(expected, result);
        }

        [Fact]
        public async Task GetCurrencyHistoryAsync_ReturnsRepositoryHistory()
        {
            IReadOnlyList<CurrencyHistoryPoint> expected = new List<CurrencyHistoryPoint>
            {
                new() { Date = DateTime.Today, BuyAt = 4.12m, SellAt = 4.08m }
            };
            var repositoryMock = new Mock<INbpJsonRepository>();
            repositoryMock
                .Setup(repository => repository.GetCurrencyHistoryAsync("EUR", 30))
                .ReturnsAsync(expected);

            var service = new NbpJsonService(repositoryMock.Object);

            var result = await service.GetCurrencyHistoryAsync("EUR", 30);

            Assert.Same(expected, result);
        }
    }
}
