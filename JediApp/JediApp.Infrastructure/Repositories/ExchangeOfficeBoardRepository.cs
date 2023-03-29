using JediApp.Database.Domain;
using JediApp.Database.Interface;

namespace JediApp.Database.Repositories
{
    public class ExchangeOfficeBoardRepository : IExchangeOfficeBoardRepository
    {
        //private readonly string fileName = "..//..//..//..//ExchangeOfficeBoard.csv"; //może przeniść do klasy statycznej ???
        private readonly string fileName = "..//ExchangeOfficeBoard.csv"; //może przeniść do klasy statycznej ???
 
        public Currency AddCurrency(Currency currency)
        {
            var id = Guid.NewGuid();
            using (StreamWriter file = new StreamWriter(fileName, true))
            {
                file.WriteLine($"{id};{currency.Name};{currency.ShortName};{currency.Country};{currency.BuyAt};{currency.SellAt}");
            }

            return new Currency { Id = id, Name = currency.Name, ShortName = currency.ShortName, Country = currency.Country, BuyAt = currency.BuyAt, SellAt = currency.SellAt };
    
    }

        public List<Currency> GetAllCurrencies()
        {
            List<Currency> currencies = new();

            if (!File.Exists(fileName))
                return new List<Currency>();

            var currenciesFromFile = File.ReadAllLines(fileName);
            foreach (var line in currenciesFromFile)
            {
                var columns = line.Split(';');
                Guid.TryParse(columns[0], out var newGuid);
                decimal.TryParse(columns[4], out var buy);
                decimal.TryParse(columns[5], out var sell);
                currencies.Add(new Currency { Id = newGuid, Name = columns[1], ShortName = columns[2], Country = columns[3], BuyAt = buy, SellAt = sell });
            }
            return currencies.OrderBy(c => c.ShortName).ToList();
        }

        public Currency GetCurrencyById(Guid id)
        {
            List<Currency> currencies = GetAllCurrencies();

            return currencies.SingleOrDefault(x => x.Id == id);
        }

        public List<Currency> BrowseCurrency(string query)
        {
            List<Currency> currencies = GetAllCurrencies();

            return currencies.Where(x => x.Name.ToLowerInvariant().Contains(query.ToLowerInvariant()) 
                                      || x.ShortName.ToLowerInvariant().Contains(query.ToLowerInvariant())
                                      || x.Country.ToLowerInvariant().Contains(query.ToLowerInvariant())).ToList();
        }

        public bool UpdateCurrency(Guid id, Currency currencyToEdit)
        {

            try
            {
                var allCurriences = GetAllCurrencies().Where(c => c.Id != id).ToList();
                File.Delete(fileName);
                using (StreamWriter file = new StreamWriter(fileName, true))
                {
                    foreach (var currency in allCurriences)
                    {
                        file.WriteLine($"{currency.Id};{currency.Name};{currency.ShortName};{currency.Country};{currency.BuyAt};{currency.SellAt}");
                    }

                    file.WriteLine($"{currencyToEdit.Id};{currencyToEdit.Name};{currencyToEdit.ShortName};{currencyToEdit.Country};{currencyToEdit.BuyAt};{currencyToEdit.SellAt}");
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool DeleteCurrency(Guid id)
        {
            
            try
            {
                var allCurriences = GetAllCurrencies().Where(c => c.Id != id).ToList();
                File.Delete(fileName);
                using (StreamWriter file = new StreamWriter(fileName, true))
                {
                    foreach (var currency in allCurriences)
                    {
                        file.WriteLine($"{currency.Id};{currency.Name};{currency.ShortName};{currency.Country};{currency.BuyAt};{currency.SellAt}");
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Currency UpdateCurrency(Currency currency)
        {
            throw new NotImplementedException();
        }
    }
}