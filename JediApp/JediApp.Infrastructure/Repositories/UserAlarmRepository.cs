using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Web.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace JediApp.Infrastructure.Repositories
{
    public class UserAlarmRepository : IUserAlarmsRepository
    {
        private readonly JediAppDbContext _jediAppDb;

        public UserAlarmRepository(JediAppDbContext jediAppDb)
        {
            _jediAppDb = jediAppDb;
        }

        public void AddUserAlarm(UserAlarm alarm)
        {
            _jediAppDb.UserAlarms.Add(alarm);
            _jediAppDb.SaveChanges();
        }

        public void DeleteUserAlarm(string alarmId)
        {
            var alarmIdGuid = Guid.Parse(alarmId);
            var alarm = _jediAppDb.UserAlarms.Where(a => a.Id == alarmIdGuid).SingleOrDefault();

            _jediAppDb.Remove(alarm);
            _jediAppDb.SaveChanges();
        }

        public void EditUserAlarm(UserAlarm alarm)
        {
            var alarmUpdated = _jediAppDb.UserAlarms.Where(a => a.Id == alarm.Id).SingleOrDefault();
            alarmUpdated.AlarmBuyAt = alarm.AlarmBuyAt;
            alarmUpdated.AlarmSellAt = alarm.AlarmSellAt;

            _jediAppDb.SaveChanges();
        }

        public UserAlarm GetUserAlarm(string alarmId)
        {
            var alarmIdGuid = Guid.Parse(alarmId);
            return _jediAppDb.UserAlarms.Where(a => a.Id == alarmIdGuid).SingleOrDefault();
        }

        public IEnumerable<UserAlarm> GetUserAlarms()
        {
            return _jediAppDb.UserAlarms.Include("User");
        }

        public IEnumerable<UserAlarm> GetUserAlarms(string userId)
        {
            return _jediAppDb.UserAlarms.Where(a => a.UserId == userId);
        }
    }
}
