using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Web.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace JediApp.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly JediAppDbContext _jediAppDb;

        public UserRepository(JediAppDbContext jediAppDb)
        {
            _jediAppDb = jediAppDb;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users =await _jediAppDb.Users.ToListAsync();

            return users;
        }
        public async Task<User> GetUserById(string id)
        {
            var user = await _jediAppDb.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public void DeleteUser(User user)
        {
            _jediAppDb.Remove(user);

            _jediAppDb.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
