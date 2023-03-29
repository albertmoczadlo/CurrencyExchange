using JediApp.Database.Domain;

namespace JediApp.Services.Services
{
    public interface IUserAlarmsService
    {
        public void ExecuteAlarms(List<Currency> currencies);
    }
}
