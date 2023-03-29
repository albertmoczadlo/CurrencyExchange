using JediApp.Database.Domain;

namespace JediApp.Database.Interface
{
    public interface IUserAlarmsRepository
    {
        public UserAlarm GetUserAlarm(string alarmId);
        public void AddUserAlarm(UserAlarm alarm);
        public void EditUserAlarm(UserAlarm alarm);
        public void DeleteUserAlarm(string alarmId);
        public IEnumerable<UserAlarm> GetUserAlarms();
        public IEnumerable<UserAlarm> GetUserAlarms(string userId);
    }
}
