using VistulaExchange.Database.Domain;

namespace VistulaExchange.Database.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string id);
        Task<IReadOnlyList<User>> GetAllUsersAsync();
        Task<int> GetUsersCountAsync();
        Task<int> GetConfirmedUsersCountAsync();
        Task DeleteUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
    }
}
