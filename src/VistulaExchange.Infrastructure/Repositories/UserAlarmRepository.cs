using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace VistulaExchange.Infrastructure.Repositories
{
    public class UserAlarmRepository : IUserAlarmsRepository
    {
        private readonly VistulaExchangeDbContext _jediAppDb;

        public UserAlarmRepository(VistulaExchangeDbContext jediAppDb)
        {
            _jediAppDb = jediAppDb;
        }

        public async Task AddUserAlarmAsync(UserAlarm alarm)
        {
            await _jediAppDb.UserAlarms.AddAsync(alarm);
            await _jediAppDb.SaveChangesAsync();
        }

        public async Task DeleteUserAlarmAsync(string alarmId)
        {
            var alarmIdGuid = Guid.Parse(alarmId);
            var alarm = await _jediAppDb.UserAlarms.SingleOrDefaultAsync(a => a.Id == alarmIdGuid);

            if (alarm is null)
            {
                return;
            }

            _jediAppDb.Remove(alarm);
            await _jediAppDb.SaveChangesAsync();
        }

        public async Task EditUserAlarmAsync(UserAlarm alarm)
        {
            var alarmUpdated = await _jediAppDb.UserAlarms.SingleOrDefaultAsync(a => a.Id == alarm.Id);

            if (alarmUpdated is null)
            {
                return;
            }

            alarmUpdated.AlarmBuyAt = alarm.AlarmBuyAt;
            alarmUpdated.AlarmSellAt = alarm.AlarmSellAt;
            alarmUpdated.ShortName = alarm.ShortName;
            alarmUpdated.CurrencyName = alarm.CurrencyName;

            await _jediAppDb.SaveChangesAsync();
        }

        public async Task<UserAlarm> GetUserAlarmAsync(string alarmId)
        {
            var alarmIdGuid = Guid.Parse(alarmId);
            return await _jediAppDb.UserAlarms.SingleOrDefaultAsync(a => a.Id == alarmIdGuid);
        }

        public async Task<List<UserAlarm>> GetUserAlarmsAsync()
        {
            return await _jediAppDb.UserAlarms
                .Include(a => a.User)
                .ToListAsync();
        }

        public async Task<List<UserAlarm>> GetUserAlarmsAsync(string userId)
        {
            return await _jediAppDb.UserAlarms
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }
    }
}
