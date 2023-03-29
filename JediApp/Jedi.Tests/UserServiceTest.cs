using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Database.Repositories;
using JediApp.Services.Services;
using JediApp.Services.Services.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Jedi.Tests
{
    public class UserServiceTest
    {
        [Fact]
        public async Task UpdateUser_ReturnsUpdateUser()
        {
            //Arrenge

            var user = new User
            {
                Id = "1",
                FirstName = "Test",
                LastName = "Test2",
                UserName = "Test3",
                Email = "test@test"
            };

            var userMock = new Mock<IUserRepository>();

            userMock.Setup(e =>e.GetUserById(user.Id)).ReturnsAsync(user);

            var userService = new UserService(userMock.Object);

            var userAfterchange = new User
            {
                Id = "1",
                FirstName = "Last",
                LastName = "Test2",
                UserName = "Test3",
                Email = "last@test"
            };

            var result = await userService.UpdateUser(userAfterchange);
            //Assert

            Assert.Equal("Last", result.FirstName);
        }

    }
}
