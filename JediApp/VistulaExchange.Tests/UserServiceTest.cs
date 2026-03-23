using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Service;
using Moq;
using Xunit;

namespace VistulaExchange.Tests
{
    public class UserServiceTest
    {
        [Fact]
        public async Task UpdateUserAsync_ReturnsUpdatedUser()
        {
            var updatedUser = new User
            {
                Id = "1",
                FirstName = "Last",
                LastName = "Test2",
                UserName = "Test3",
                Email = "last@test"
            };

            var userMock = new Mock<IUserRepository>();
            userMock
                .Setup(e => e.UpdateUserAsync(updatedUser))
                .ReturnsAsync(updatedUser);

            var userService = new UserService(userMock.Object);

            var result = await userService.UpdateUserAsync(updatedUser);

            Assert.NotNull(result);
            Assert.Equal("Last", result!.FirstName);
            userMock.Verify(e => e.UpdateUserAsync(updatedUser), Times.Once);
        }
    }
}
