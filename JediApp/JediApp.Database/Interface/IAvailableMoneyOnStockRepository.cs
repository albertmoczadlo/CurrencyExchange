using JediApp.Database.Domain;

namespace JediApp.Database.Interface
{
    public interface IAvailableMoneyOnStockRepository
    {
        void AddMoneyToStock(MoneyOnStock moneyOnStock);
        List<MoneyOnStock> GetAvailableMoneyOnStock();
        string SubtractMoneyFromStock(MoneyOnStock moneyOnStock);
    }

}
