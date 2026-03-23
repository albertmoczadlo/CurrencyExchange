using VistulaExchange.Database.Domain;

namespace VistulaExchange.Database.Interface
{
    public interface IAvailableMoneyOnStockRepository
    {
        Task AddMoneyToStockAsync(MoneyOnStock moneyOnStock);
        Task<List<MoneyOnStock>> GetAvailableMoneyOnStockAsync();
        Task<string> SubtractMoneyFromStockAsync(MoneyOnStock moneyOnStock);
    }

}
