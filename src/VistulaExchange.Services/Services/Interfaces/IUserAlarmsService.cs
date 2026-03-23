using VistulaExchange.Database.Domain;

namespace VistulaExchange.Services.Services.Interfaces
{
    public interface IUserAlarmsService
    {
        Task ExecuteAlarmsAsync(List<Currency> currencies);
        Task<List<UserAlarm>> GetUserAlarmsAsync(string userId);
        Task<UserAlarm> GetUserAlarmAsync(string alarmId);
        Task AddUserAlarmAsync(UserAlarm alarm);
        Task EditUserAlarmAsync(UserAlarm alarm);
        Task DeleteUserAlarmAsync(string alarmId);
    }
}
