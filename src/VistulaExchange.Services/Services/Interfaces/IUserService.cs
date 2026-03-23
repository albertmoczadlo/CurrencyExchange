using VistulaExchange.Database.Domain;

namespace VistulaExchange.Services.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> UpdateUserAsync(User user);
    }
}
