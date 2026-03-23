using VistulaExchange.Database.Domain;

namespace VistulaExchange.Database.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task DeleteUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
    }
}
