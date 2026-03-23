using Moq;
using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Service;
using Xunit;

namespace VistulaExchange.Tests
{
    public class TransactionHistoryServiceTest
    {
        [Fact]
        public async Task AddTransactionAsync_ReturnsRepositoryResult()
        {
            var transaction = new TransactionHistory { UserId = "user-1", Description = "Buy EUR" };
            var repositoryMock = new Mock<ITransactionHistoryRepository>();
            repositoryMock
                .Setup(repository => repository.AddTransactionAsync(transaction))
                .ReturnsAsync(true);

            var service = new TransactionHistoryService(repositoryMock.Object);

            var result = await service.AddTransactionAsync(transaction);

            Assert.True(result);
        }

        [Fact]
        public async Task GetUserHistoryByUserIdAsync_ReturnsRepositoryHistory()
        {
            var expected = new List<TransactionHistory>
            {
                new() { UserId = "user-1", Description = "Sell USD" }
            };
            var repositoryMock = new Mock<ITransactionHistoryRepository>();
            repositoryMock
                .Setup(repository => repository.GetUserHistoryByUserIdAsync("user-1"))
                .ReturnsAsync(expected);

            var service = new TransactionHistoryService(repositoryMock.Object);

            var result = await service.GetUserHistoryByUserIdAsync("user-1");

            Assert.Same(expected, result);
        }

        [Fact]
        public async Task GetAllUsersHistoriesAsync_ReturnsRepositoryHistory()
        {
            var expected = new List<TransactionHistory>
            {
                new() { UserId = "user-1", Description = "Deposit" },
                new() { UserId = "user-2", Description = "Withdrawal" }
            };
            var repositoryMock = new Mock<ITransactionHistoryRepository>();
            repositoryMock
                .Setup(repository => repository.GetAllUsersHistoriesAsync())
                .ReturnsAsync(expected);

            var service = new TransactionHistoryService(repositoryMock.Object);

            var result = await service.GetAllUsersHistoriesAsync();

            Assert.Same(expected, result);
        }
    }
}
