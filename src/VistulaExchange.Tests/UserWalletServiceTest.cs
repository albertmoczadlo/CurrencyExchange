using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Service;
using Moq;
using Xunit;

namespace VistulaExchange.Tests
{
    public class UserWalletServiceTest
    {

        [Fact]
        public async Task GetCurrencyBalanceByIdAsync_ReturnsZeroCurrencyAmount_ForNoWalletPosition()
        {
            var moqUserWalletRepository = new Mock<IUserWalletRepository>();

            var newUserWalletService = new UserWalletService(moqUserWalletRepository.Object);

            var wallet = new Wallet
            {
                WalletPositions = new List<WalletPosition>()
            };

            moqUserWalletRepository.Setup(x => x.GetWalletAsync(It.IsAny<string>())).ReturnsAsync(wallet);

            var result = await newUserWalletService.GetCurrencyBalanceByIdAsync("1", new Guid());

            Assert.Equal(0, result.CurrencyAmount);
        }

        [Fact]
        public async Task GetCurrencyBalanceByIdAsync_ReturnsCurrentsAmount_ForExistingWalletPosition()
        {
            var moqUserWalletRepository = new Mock<IUserWalletRepository>();

            var newUserWalletService = new UserWalletService(moqUserWalletRepository.Object);

            var guid = Guid.NewGuid();

            var wallet = new Wallet
            {
                WalletPositions = new List<WalletPosition>()
                {
                      new WalletPosition
                      {
                           Id = guid ,
                           CurrencyAmount = 2.3M,
                           Currency = new Currency()
                           {
                               Id = guid
                           }
                      }
                }
            };

            moqUserWalletRepository.Setup(x => x.GetWalletAsync(It.IsAny<string>())).ReturnsAsync(wallet);

            var result = await newUserWalletService.GetCurrencyBalanceByIdAsync("1", guid);

            Assert.Equal(2.3M , result.CurrencyAmount);
            Assert.Equal(guid, result.Id);
        }
    }
}
