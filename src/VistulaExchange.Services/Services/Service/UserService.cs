using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Interfaces;

namespace VistulaExchange.Services.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IReadOnlyList<User>> GetAllUsersAsync()
        {
            return _userRepository.GetAllUsersAsync();
        }

        public Task<User> GetUserByIdAsync(string id)
        {
            return _userRepository.GetUserByIdAsync(id);
        }

        public Task<int> GetUsersCountAsync()
        {
            return _userRepository.GetUsersCountAsync();
        }

        public Task<int> GetConfirmedUsersCountAsync()
        {
            return _userRepository.GetConfirmedUsersCountAsync();
        }

        public Task DeleteUserAsync(User user)
        {
            return _userRepository.DeleteUserAsync(user);
        }

        public Task<User> UpdateUserAsync(User user)
        {
            return _userRepository.UpdateUserAsync(user);
        }
    }
}
