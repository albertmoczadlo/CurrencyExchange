using JediApp.Database.Domain;

namespace JediApp.Database.Repositories
{
    public interface IUserRepository
    {
        User GetUserById(Guid id);
        User GetUserByLogin(string login);
        List<User> GetAllUsers();
        List<User> BrowseUsers(string query);
        User AddUser(User user);
        User GetLoginPassword(string login, string password);
    }
}
