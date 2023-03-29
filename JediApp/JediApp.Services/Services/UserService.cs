using JediApp.Database.Domain;
using JediApp.Database.Interface;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace JediApp.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> UpdateUser(User user)
        {
            var users = await _userRepository.GetUserById(user.Id);

            users.FirstName = user.FirstName;
            users.LastName = user.LastName;
            users.UserName = user.UserName;
            users.Email = user.Email;

            return users;
        }

    }
}
