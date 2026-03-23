using Moq;
using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Service;
using Xunit;

namespace VistulaExchange.Tests
{
    public class ExchangeOfficeServiceTest
    {
        [Fact]
        public void GetAllExchangeOffices_ReturnsRepositoryResult()
        {
            var expected = new List<ExchangeOffice>
            {
                new() { Id = Guid.NewGuid(), Name = "Warsaw HQ" }
            };
            var repositoryMock = new Mock<IExchangeOfficeRepository>();
            repositoryMock
                .Setup(repository => repository.GetAllExchangeOffices())
                .Returns(expected);

            var service = new ExchangeOfficeService(repositoryMock.Object);

            var result = service.GetAllExchangeOffices();

            Assert.Same(expected, result);
        }

        [Fact]
        public void GetExchangeOfficeById_ReturnsRepositoryResult()
        {
            var id = Guid.NewGuid();
            var expected = new ExchangeOffice { Id = id, Name = "Krakow Desk" };
            var repositoryMock = new Mock<IExchangeOfficeRepository>();
            repositoryMock
                .Setup(repository => repository.GetExchangeOfficeById(id))
                .Returns(expected);

            var service = new ExchangeOfficeService(repositoryMock.Object);

            var result = service.GetExchangeOfficeById(id);

            Assert.Same(expected, result);
        }

        [Fact]
        public void UpdateExchangeOffice_ReturnsFalse_WhenOfficeDoesNotExist()
        {
            var id = Guid.NewGuid();
            var repositoryMock = new Mock<IExchangeOfficeRepository>();
            repositoryMock
                .Setup(repository => repository.GetAllExchangeOffices())
                .Returns(new List<ExchangeOffice>());

            var service = new ExchangeOfficeService(repositoryMock.Object);

            var result = service.UpdateExchangeOffice(id, new ExchangeOffice { Id = id });

            Assert.False(result);
            repositoryMock.Verify(repository => repository.UpdateExchangeOffice(It.IsAny<Guid>(), It.IsAny<ExchangeOffice>()), Times.Never);
        }

        [Fact]
        public void UpdateExchangeOffice_ReturnsRepositoryResult_WhenOfficeExists()
        {
            var id = Guid.NewGuid();
            var edited = new ExchangeOffice { Id = id, Name = "Updated Office" };
            var repositoryMock = new Mock<IExchangeOfficeRepository>();
            repositoryMock
                .Setup(repository => repository.GetAllExchangeOffices())
                .Returns(new List<ExchangeOffice>
                {
                    new() { Id = id, Name = "Existing Office" }
                });
            repositoryMock
                .Setup(repository => repository.UpdateExchangeOffice(id, edited))
                .Returns(true);

            var service = new ExchangeOfficeService(repositoryMock.Object);

            var result = service.UpdateExchangeOffice(id, edited);

            Assert.True(result);
            repositoryMock.Verify(repository => repository.UpdateExchangeOffice(id, edited), Times.Once);
        }
    }
}
