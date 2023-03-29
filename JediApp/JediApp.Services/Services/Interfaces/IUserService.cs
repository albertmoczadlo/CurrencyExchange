using JediApp.Database.Domain;

namespace JediApp.Services.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> UpdateUser(User user);
    }
}
