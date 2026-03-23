using VistulaExchange.Database.Domain;

namespace VistulaExchange.Database.Interface
{
    public interface IUserAlarmsRepository
    {
        Task<UserAlarm> GetUserAlarmAsync(string alarmId);
        Task AddUserAlarmAsync(UserAlarm alarm);
        Task EditUserAlarmAsync(UserAlarm alarm);
        Task DeleteUserAlarmAsync(string alarmId);
        Task<List<UserAlarm>> GetUserAlarmsAsync();
        Task<List<UserAlarm>> GetUserAlarmsAsync(string userId);
    }
}
