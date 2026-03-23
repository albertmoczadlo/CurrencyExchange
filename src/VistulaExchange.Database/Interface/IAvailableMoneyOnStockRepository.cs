using VistulaExchange.Database.Domain;

namespace VistulaExchange.Database.Interface
{
    public interface IAvailableMoneyOnStockRepository
    {
        void AddMoneyToStock(MoneyOnStock moneyOnStock);
        List<MoneyOnStock> GetAvailableMoneyOnStock();
        string SubtractMoneyFromStock(MoneyOnStock moneyOnStock);
    }

}
