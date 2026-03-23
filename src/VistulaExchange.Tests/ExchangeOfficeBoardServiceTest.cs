using Moq;
using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Service;
using Xunit;

namespace VistulaExchange.Tests
{
    public class ExchangeOfficeBoardServiceTest
    {
        [Fact]
        public void AddCurrency_UsesRepositoryAdd_WhenShortNameIsUnique()
        {
            var repositoryMock = new Mock<IExchangeOfficeBoardRepository>();
            var currency = new Currency { ShortName = "EUR" };
            repositoryMock.Setup(repository => repository.GetAllCurrencies()).Returns(new List<Currency>());
            repositoryMock.Setup(repository => repository.AddCurrency(currency)).Returns(currency);
            var service = new ExchangeOfficeBoardService(repositoryMock.Object);

            var result = service.AddCurrency(currency);

            Assert.Same(currency, result);
            repositoryMock.Verify(repository => repository.AddCurrency(currency), Times.Once);
            repositoryMock.Verify(repository => repository.UpdateCurrency(It.IsAny<Currency>()), Times.Never);
        }

        [Fact]
        public void AddCurrency_UsesRepositoryUpdate_WhenShortNameAlreadyExists()
        {
            var repositoryMock = new Mock<IExchangeOfficeBoardRepository>();
            var currency = new Currency { ShortName = "EUR" };
            repositoryMock.Setup(repository => repository.GetAllCurrencies()).Returns(new List<Currency> { new() { ShortName = "EUR" } });
            repositoryMock.Setup(repository => repository.UpdateCurrency(currency)).Returns(currency);
            var service = new ExchangeOfficeBoardService(repositoryMock.Object);

            var result = service.AddCurrency(currency);

            Assert.Same(currency, result);
            repositoryMock.Verify(repository => repository.UpdateCurrency(currency), Times.Once);
            repositoryMock.Verify(repository => repository.AddCurrency(It.IsAny<Currency>()), Times.Never);
        }

        [Fact]
        public void UpdateCurrency_ReturnsFalse_WhenCurrencyDoesNotExist()
        {
            var repositoryMock = new Mock<IExchangeOfficeBoardRepository>();
            repositoryMock.Setup(repository => repository.GetAllCurrencies()).Returns(new List<Currency>());
            var service = new ExchangeOfficeBoardService(repositoryMock.Object);

            var result = service.UpdateCurrency(Guid.NewGuid(), new Currency());

            Assert.False(result);
            repositoryMock.Verify(repository => repository.UpdateCurrency(It.IsAny<Guid>(), It.IsAny<Currency>()), Times.Never);
        }

        [Fact]
        public void DeleteCurrencyByShortName_ReturnsFalse_WhenCurrencyDoesNotExist()
        {
            var repositoryMock = new Mock<IExchangeOfficeBoardRepository>();
            repositoryMock.Setup(repository => repository.GetAllCurrencies()).Returns(new List<Currency>());
            var service = new ExchangeOfficeBoardService(repositoryMock.Object);

            var result = service.DeleteCurrencyByShortName("EUR");

            Assert.False(result);
            repositoryMock.Verify(repository => repository.DeleteCurrency(It.IsAny<Guid>()), Times.Never);
        }
    }
}
