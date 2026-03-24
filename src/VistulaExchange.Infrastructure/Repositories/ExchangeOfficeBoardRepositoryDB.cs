using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Infrastructure.Persistence;

namespace VistulaExchange.Infrastructure.Repositories
{
    public class ExchangeOfficeBoardRepositoryDB : IExchangeOfficeBoardRepository
    {
        private readonly VistulaExchangeDbContext _dbContext;

        public ExchangeOfficeBoardRepositoryDB(VistulaExchangeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Currency AddCurrency(Currency currency)
        {
            currency.ExchangeOfficeBoardId = _dbContext.ExchangeOfficeBoards.FirstOrDefault().Id;

            var currecyDic = _dbContext.CurrencyDictionaries.Where(cd => cd.ShortName.ToLower().Equals(currency.ShortName.ToLower())).FirstOrDefault();
            if(currecyDic != null)
            {
                currency.Name = currecyDic.Name;
                currency.ShortName = currecyDic.ShortName;
                currency.Country = currecyDic.Country;
            }

            _dbContext.Add(currency);
            _dbContext.SaveChanges();

            return currency;

        }

        public Currency UpdateCurrency(Currency currency)
        {
            var currencyFromDb = _dbContext.Currencys.Where(c => c.ShortName == currency.ShortName).FirstOrDefault();
            currencyFromDb.SellAt = currency.SellAt;
            currencyFromDb.BuyAt = currency.BuyAt;

            _dbContext.Update(currencyFromDb);
            _dbContext.SaveChanges();

            return currencyFromDb;
        }

        public List<Currency> GetAllCurrencies()
        {
            var office = _dbContext.ExchangeOffices.FirstOrDefault();
            if (office is null)
            {
                office = new ExchangeOffice
                {
                    Id = Guid.NewGuid(),
                    Name = "Jedi Web Exchange Office",
                    Address = "ul. Wiejska 4/6/8 00-902 Warszawa",
                    Markup = 5
                };

                _dbContext.Add(office);
                _dbContext.SaveChanges();
            }

            var officeBoard = _dbContext.ExchangeOfficeBoards.FirstOrDefault();
            if(officeBoard is null)
            {
                officeBoard = new ExchangeOfficeBoard { Id = Guid.NewGuid(), ExchangeOfficeId = office.Id };
                _dbContext.Add(officeBoard);
                _dbContext.SaveChanges();

            }

            var pln = _dbContext.Currencys.Where(c => (c.ShortName.ToLower()).Equals("pln")).FirstOrDefault();
            if (pln is null)
            {
                pln = new Currency
                {
                    Id = Guid.NewGuid(),
                    Name = "Polish zloty",
                    ShortName = "PLN",
                    Country = "Poland",
                    BuyAt = 1,
                    SellAt = 1,
                    ExchangeOfficeBoardId = _dbContext.ExchangeOfficeBoards.FirstOrDefault().Id
                };
                _dbContext.Add(pln);
            }
            _dbContext.SaveChanges();

            return _dbContext.Currencys.OrderBy(c => c.ShortName).ToList();

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
                var currency = GetCurrencyById(id);

                currency.Name = currencyToEdit.Name;
                currency.ShortName = currencyToEdit.ShortName;
                currency.Country = currencyToEdit.Country;
                currency.BuyAt = currencyToEdit.BuyAt;
                currency.SellAt = currencyToEdit.SellAt;

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[ExchangeOfficeBoardRepository] UpdateCurrency failed for id={id}: {ex}");
                return false;
            }
            return true;
        }

        public bool DeleteCurrency(Guid id)
        {
            try
            {
                var currency = GetCurrencyById(id);
                _dbContext.Remove(currency);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[ExchangeOfficeBoardRepository] DeleteCurrency failed for id={id}: {ex}");
                return false;
            }
            return true;
        }
    }
}