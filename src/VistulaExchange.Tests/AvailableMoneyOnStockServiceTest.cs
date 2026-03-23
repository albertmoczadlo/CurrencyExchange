using Moq;
using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Service;
using Xunit;

namespace VistulaExchange.Tests
{
    public class AvailableMoneyOnStockServiceTest
    {
        [Fact]
        public async Task GetAvailableMoneyOnStockAsync_ReturnsRepositoryResult()
        {
            var expected = new List<MoneyOnStock>
            {
                new() { CurrencyName = "EUR", Value = 100m }
            };
            var repositoryMock = new Mock<IAvailableMoneyOnStockRepository>();
            repositoryMock
                .Setup(repository => repository.GetAvailableMoneyOnStockAsync())
                .ReturnsAsync(expected);

            var service = new AvailableMoneyOnStockService(repositoryMock.Object);

            var result = await service.GetAvailableMoneyOnStockAsync();

            Assert.Same(expected, result);
        }

        [Fact]
        public async Task AddMoneyToStockAsync_ForwardsMoneyPositionToRepository()
        {
            var position = new MoneyOnStock { CurrencyName = "USD", Value = 200m };
            var repositoryMock = new Mock<IAvailableMoneyOnStockRepository>();

            var service = new AvailableMoneyOnStockService(repositoryMock.Object);

            await service.AddMoneyToStockAsync(position);

            repositoryMock.Verify(repository => repository.AddMoneyToStockAsync(position), Times.Once);
        }

        [Fact]
        public async Task SubtractMoneyFromStockAsync_ReturnsRepositoryMessage()
        {
            var position = new MoneyOnStock { CurrencyName = "PLN", Value = 50m };
            var repositoryMock = new Mock<IAvailableMoneyOnStockRepository>();
            repositoryMock
                .Setup(repository => repository.SubtractMoneyFromStockAsync(position))
                .ReturnsAsync("OK");

            var service = new AvailableMoneyOnStockService(repositoryMock.Object);

            var result = await service.SubtractMoneyFromStockAsync(position);

            Assert.Equal("OK", result);
        }
    }
}
