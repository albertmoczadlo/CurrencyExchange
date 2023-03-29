using JediApp.Database.Domain;

namespace JediApp.Database.Repositories
{
    public interface IAvailableMoneyOnStockRepository
    {
        void AddMoneyToStock(MoneyOnStock moneyOnStock);
        List<MoneyOnStock> GetAvailableMoneyOnStock();
    }
    
}
