using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Web.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace VistulaExchange.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly VistulaExchangeDbContext _jediAppDb;

        public UserRepository(VistulaExchangeDbContext jediAppDb)
        {
            _jediAppDb = jediAppDb;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _jediAppDb.Users.ToListAsync();

            return users;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var user = await _jediAppDb.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task DeleteUserAsync(User user)
        {
            _jediAppDb.Remove(user);

            await _jediAppDb.SaveChangesAsync();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var userToUpdate = await _jediAppDb.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (userToUpdate is null)
            {
                return null;
            }

            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.UserName = user.UserName;
            userToUpdate.Email = user.Email;
            userToUpdate.NormalizedUserName = user.UserName?.ToUpperInvariant();
            userToUpdate.NormalizedEmail = user.Email?.ToUpperInvariant();

            await _jediAppDb.SaveChangesAsync();

            return userToUpdate;
        }
    }
}
