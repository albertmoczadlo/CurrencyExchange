using JediApp.Database.Domain;

namespace JediApp.Services.Services
{
    public interface IUserService
    {
        Task<User> UpdateUser(User user);
    }
}
