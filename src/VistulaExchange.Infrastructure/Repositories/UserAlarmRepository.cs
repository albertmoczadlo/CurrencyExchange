using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace VistulaExchange.Infrastructure.Repositories
{
    public class UserAlarmRepository : IUserAlarmsRepository
    {
        private readonly VistulaExchangeDbContext _dbContext;

        public UserAlarmRepository(VistulaExchangeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUserAlarmAsync(UserAlarm alarm)
        {
            await _dbContext.UserAlarms.AddAsync(alarm);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAlarmAsync(string alarmId)
        {
            if (!Guid.TryParse(alarmId, out var alarmIdGuid))
            {
                return;
            }

            var alarm = await _dbContext.UserAlarms.SingleOrDefaultAsync(a => a.Id == alarmIdGuid);

            if (alarm is null)
            {
                return;
            }

            _dbContext.Remove(alarm);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditUserAlarmAsync(UserAlarm alarm)
        {
            var alarmUpdated = await _dbContext.UserAlarms.SingleOrDefaultAsync(a => a.Id == alarm.Id);

            if (alarmUpdated is null)
            {
                return;
            }

            alarmUpdated.AlarmBuyAt = alarm.AlarmBuyAt;
            alarmUpdated.AlarmSellAt = alarm.AlarmSellAt;
            alarmUpdated.ShortName = alarm.ShortName;
            alarmUpdated.CurrencyName = alarm.CurrencyName;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserAlarm> GetUserAlarmAsync(string alarmId)
        {
            if (!Guid.TryParse(alarmId, out var alarmIdGuid))
            {
                return null;
            }

            return await _dbContext.UserAlarms.SingleOrDefaultAsync(a => a.Id == alarmIdGuid);
        }

        public async Task<List<UserAlarm>> GetUserAlarmsAsync()
        {
            return await _dbContext.UserAlarms
                .Include(a => a.User)
                .ToListAsync();
        }

        public async Task<List<UserAlarm>> GetUserAlarmsAsync(string userId)
        {
            return await _dbContext.UserAlarms
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }
    }
}
