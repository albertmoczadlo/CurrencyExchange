using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace VistulaExchange.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly VistulaExchangeDbContext _dbContext;

        public UserRepository(VistulaExchangeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<User>> GetAllUsersAsync()
        {
            var users = await _dbContext.Users.ToListAsync();

            return users;
        }

        public Task<int> GetUsersCountAsync()
        {
            return _dbContext.Users.CountAsync();
        }

        public Task<int> GetConfirmedUsersCountAsync()
        {
            return _dbContext.Users.CountAsync(x => x.EmailConfirmed);
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task DeleteUserAsync(User user)
        {
            _dbContext.Remove(user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var userToUpdate = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

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

            await _dbContext.SaveChangesAsync();

            return userToUpdate;
        }
    }
}
