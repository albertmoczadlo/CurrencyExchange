using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Web.Areas.Identity.Data;

namespace JediApp.Database.Repositories
{
    public class AvailableMoneyOnStockRepository : IAvailableMoneyOnStockRepository
    {
        private readonly JediAppDbContext _jediAppDb;

        public AvailableMoneyOnStockRepository(JediAppDbContext jediAppDb)
        {
            _jediAppDb = jediAppDb;
        }

        public void AddMoneyToStock(MoneyOnStock moneyOnStock)
        {
            var office = _jediAppDb.ExchangeOffices.FirstOrDefault();
            moneyOnStock.ExchangeOffice = office;

            var currentMoneyOnStockForCurrency = _jediAppDb.MoneyOnStocks.Where(a => a.CurrencyName == moneyOnStock.CurrencyName).SingleOrDefault();
            if (currentMoneyOnStockForCurrency != null)
            {
                currentMoneyOnStockForCurrency.Value = currentMoneyOnStockForCurrency.Value + moneyOnStock.Value;
            }
            else
            {
                _jediAppDb.MoneyOnStocks.Add(moneyOnStock);
            }

            _jediAppDb.SaveChanges();
        }

        public string SubtractMoneyFromStock(MoneyOnStock moneyOnStock)
        {
            var office = _jediAppDb.ExchangeOffices.FirstOrDefault();
            moneyOnStock.ExchangeOffice = office;

            var currentMoneyOnStockForCurrency = _jediAppDb.MoneyOnStocks.Where(a => a.CurrencyName == moneyOnStock.CurrencyName).SingleOrDefault();

            if (currentMoneyOnStockForCurrency != null)
            {
                currentMoneyOnStockForCurrency.Value = currentMoneyOnStockForCurrency.Value - moneyOnStock.Value;
                if (currentMoneyOnStockForCurrency.Value < 0)
                {
                    return "Not enough money in the account.";
                }
            }
            else
            {
                return "No money found for this currency.";
            }

            _jediAppDb.SaveChanges();

            return " ";
        }


        public List<MoneyOnStock> GetAvailableMoneyOnStock()
        {
            return _jediAppDb.MoneyOnStocks.ToList();
        }

    }
}
