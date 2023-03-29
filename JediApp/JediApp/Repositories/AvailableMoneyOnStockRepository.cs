using JediApp.Database.Domain;

namespace JediApp.Database.Repositories
{
    public class AvailableMoneyOnStockRepository : IAvailableMoneyOnStockRepository

    {
        private readonly string fileName = "..//..//..//..//AvailableMoneyOnStock.csv";
        public void AddMoneyToStock(MoneyOnStock moneyOnStock)
        {
            var currentMoneyOnStock = GetAvailableMoneyOnStock();
            var allOtherMoneyOnStock = currentMoneyOnStock.Where(a => !a.CurrencyName.Equals(moneyOnStock.CurrencyName, StringComparison.InvariantCultureIgnoreCase));
            File.Delete(fileName);

            // Add requeste money to stock
            using (StreamWriter file = new StreamWriter(fileName, true))
            {
                var id = Guid.NewGuid();
                file.WriteLine($"{id};{moneyOnStock.Value};{moneyOnStock.CurrencyName}");
            }

            // Add all other money
            foreach (var money in allOtherMoneyOnStock)
            {
                using (StreamWriter file = new StreamWriter(fileName, true))
                {
                    var id = Guid.NewGuid();
                    file.WriteLine($"{id};{money.Value};{money.CurrencyName}");
                }
            }
        }

        public List<MoneyOnStock> GetAvailableMoneyOnStock()
        {
            var moneyOnStockList = new List<MoneyOnStock>();

            if (!File.Exists(fileName))
                return new List<MoneyOnStock>();

            var moneyOnStockFromFile = File.ReadAllLines(fileName);
            foreach (var line in moneyOnStockFromFile)
            {
                var columns = line.Split(';');
                Guid.TryParse(columns[0], out var newGuid);
                decimal.TryParse(columns[1], out var currencyValue);
                moneyOnStockList.Add(new MoneyOnStock { Id = newGuid, CurrencyName = columns[2], Value = currencyValue });
            }

            return moneyOnStockList;
        }
    }
}
