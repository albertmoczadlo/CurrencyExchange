using VistulaExchange.Database.Domain;

namespace VistulaExchange.Services.Services.Interfaces
{
    public interface IUserAlarmsService
    {
        Task ExecuteAlarmsAsync(List<Currency> currencies);
    }
}
