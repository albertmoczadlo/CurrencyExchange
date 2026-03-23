using VistulaExchange.Database.Domain;

namespace VistulaExchange.Services.Services.Interfaces
{
    public interface IAvailableMoneyOnStockService
    {
        Task<IReadOnlyList<MoneyOnStock>> GetAvailableMoneyOnStockAsync();
        Task AddMoneyToStockAsync(MoneyOnStock moneyOnStock);
        Task<string> SubtractMoneyFromStockAsync(MoneyOnStock moneyOnStock);
    }
}
