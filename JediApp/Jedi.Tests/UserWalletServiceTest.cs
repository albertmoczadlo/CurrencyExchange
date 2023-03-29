using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Services.Services;
using JediApp.Services.Services.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Jedi.Tests
{
    public class UserWalletServiceTest
    {

        [Fact]
        public void GetCurrencyBalanceById_ReturnsZeroCurrencyAmount_ForNoWalletPosition()
        {
            var moqUserWalletRepository = new Mock<IUserWalletRepository>();

            var newUserWalletService = new UserWalletService(moqUserWalletRepository.Object);

            var wallet = new Wallet
            {
                WalletPositions = new List<WalletPosition>()
            };

            moqUserWalletRepository.Setup(x => x.GetWallet(It.IsAny<string>())).Returns(wallet);

            var result = newUserWalletService.GetCurrencyBalanceById("1", new Guid());

            Assert.Equal(0, result.CurrencyAmount);
        }

        [Fact]
        public void GetCurrencyBalanceById_ReturnsCurrentsAmount_ForExistingWalletPosition()
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

            moqUserWalletRepository.Setup(x => x.GetWallet(It.IsAny<string>())).Returns(wallet);

            var result = newUserWalletService.GetCurrencyBalanceById("1", guid);

            Assert.Equal(2.3M , result.CurrencyAmount);
            Assert.Equal(guid, result.Id);
        }
    }
}
