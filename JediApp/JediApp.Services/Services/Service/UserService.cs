using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Services.Services.Interfaces;

namespace JediApp.Services.Services.Service
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
