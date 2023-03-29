using JediApp.Database.Domain;

namespace JediApp.Services.Services.Interfaces
{
    public interface IUserAlarmsService
    {
        public void ExecuteAlarms(List<Currency> currencies);
    }
}
