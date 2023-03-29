using JediApp.Database.Domain;

namespace JediApp.Database.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserById(string id);
        Task<IEnumerable<User>> GetAllUsers();
        void DeleteUser(User user);
        void UpdateUser(User user);
    }
}
