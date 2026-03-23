using VistulaExchange.Database.Domain;

namespace VistulaExchange.Services.Services.Interfaces
{
    public interface IUserService
    {
        Task<IReadOnlyList<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<int> GetUsersCountAsync();
        Task<int> GetConfirmedUsersCountAsync();
        Task DeleteUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
    }
}
