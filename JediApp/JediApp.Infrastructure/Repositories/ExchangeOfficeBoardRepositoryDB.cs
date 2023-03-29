using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Web.Areas.Identity.Data;

namespace JediApp.Database.Repositories
{
    public class ExchangeOfficeBoardRepositoryDB : IExchangeOfficeBoardRepository
    {
        private readonly JediAppDbContext _jediAppDb;

        public ExchangeOfficeBoardRepositoryDB(JediAppDbContext jediAppDb)
        {
            _jediAppDb = jediAppDb;
        }

        public Currency AddCurrency(Currency currency)
        {
            currency.ExchangeOfficeBoardId = _jediAppDb.ExchangeOfficeBoards.FirstOrDefault().Id; //tymczasowo M

            //_jediAppDb.AddAsync(currency);
            //_jediAppDb.SaveChangesAsync();

            //if (currency.ShortName.ToLower().Equals("eur"))
            //{
            //    currency.Name = "Euro";
            //    currency.ShortName = "EUR";
            //    currency.Country = "European Union (EU)";
            //}

            //if exist in dic add full data
            var currecyDic = _jediAppDb.CurrencyDictionaries.Where(cd => cd.ShortName.ToLower().Equals(currency.ShortName.ToLower())).FirstOrDefault();
            if(currecyDic != null)
            {
                currency.Name = currecyDic.Name;
                currency.ShortName = currecyDic.ShortName;
                currency.Country = currecyDic.Country;
            }

            _jediAppDb.Add(currency);
            _jediAppDb.SaveChanges();

            return currency;

        }

        public Currency UpdateCurrency(Currency currency)
        {
            var currencyFromDb = _jediAppDb.Currencys.Where(c => c.ShortName == currency.ShortName).FirstOrDefault();
            currencyFromDb.SellAt = currency.SellAt;
            currencyFromDb.BuyAt = currency.BuyAt;

            _jediAppDb.Update(currencyFromDb);
            _jediAppDb.SaveChanges();

            return currencyFromDb;
        }

        public List<Currency> GetAllCurrencies()
        {
            //tmp check if officeBoard exist
            var office = _jediAppDb.ExchangeOffices.FirstOrDefault();
            if (office is null)
            {
                office = new ExchangeOffice
                {
                    Id = Guid.NewGuid(),
                    Name = "Jedi Web Exchange Office",
                    Address = "ul. Wiejska 4/6/8 00-902 Warszawa",
                    Markup = 5
                };

                _jediAppDb.Add(office);
                _jediAppDb.SaveChanges();
            }

            var officeBoard = _jediAppDb.ExchangeOfficeBoards.FirstOrDefault();
            if(officeBoard is null)
            {
               
                officeBoard = new ExchangeOfficeBoard { Id = Guid.NewGuid(), ExchangeOfficeId = office.Id };
                _jediAppDb.Add(officeBoard);
                _jediAppDb.SaveChanges();

            }
            //end tmp
            //tmp check if currencies , DB initialization some data
            var pln = _jediAppDb.Currencys.Where(c => (c.ShortName.ToLower()).Equals("pln")).FirstOrDefault();
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
                    //Fluctuation = "Decrease",
                    //AlarmValue = 1,
                    ExchangeOfficeBoardId = _jediAppDb.ExchangeOfficeBoards.FirstOrDefault().Id
                };
                _jediAppDb.Add(pln);
            }

            //var euro = _jediAppDb.Currencys.Where(c => (c.ShortName.ToLower()).Equals("eur")).FirstOrDefault();
            //if (euro is null)
            //{
            //    euro = new Currency
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Euro",
            //        ShortName = "EUR",
            //        Country = "European Union (EU)",
            //        BuyAt = 1,
            //        SellAt = 1,
            //        ExchangeOfficeBoardId = _jediAppDb.ExchangeOfficeBoards.FirstOrDefault().Id
            //    };
            //    _jediAppDb.Add(euro);
            //}

            //var usd = _jediAppDb.Currencys.Where(c => (c.ShortName.ToLower()).Equals("usd")).FirstOrDefault();
            //if (usd is null)
            //{
            //    usd = new Currency
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "United States dollar",
            //        ShortName = "USD",
            //        Country = "United States",
            //        BuyAt = 1,
            //        SellAt = 1,
            //        ExchangeOfficeBoardId = _jediAppDb.ExchangeOfficeBoards.FirstOrDefault().Id
            //    };
            //    _jediAppDb.Add(usd);
            //}

            //var chf = _jediAppDb.Currencys.Where(c => (c.ShortName.ToLower()).Equals("chf")).FirstOrDefault();
            //if (chf is null)
            //{
            //    chf = new Currency
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Swiss franc",
            //        ShortName = "CHF",
            //        Country = "Switzerland",
            //        BuyAt = 1,
            //        SellAt = 1,
            //        ExchangeOfficeBoardId = _jediAppDb.ExchangeOfficeBoards.FirstOrDefault().Id
            //    };
            //    _jediAppDb.Add(chf);
            //}

            //var gbp = _jediAppDb.Currencys.Where(c => (c.ShortName.ToLower()).Equals("gbp")).FirstOrDefault();
            //if (gbp is null)
            //{
            //    gbp = new Currency
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "British pound",
            //        ShortName = "GBP",
            //        Country = "United Kingdom",
            //        BuyAt = 1,
            //        SellAt = 1,
            //        ExchangeOfficeBoardId = _jediAppDb.ExchangeOfficeBoards.FirstOrDefault().Id
            //    };
            //    _jediAppDb.Add(gbp);
            //}

            //var aud = _jediAppDb.Currencys.Where(c => (c.ShortName.ToLower()).Equals("aud")).FirstOrDefault();
            //if (aud is null)
            //{
            //    aud = new Currency
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Australian dollar",
            //        ShortName = "AUD",
            //        Country = "Australia",
            //        BuyAt = 1,
            //        SellAt = 1,
            //        ExchangeOfficeBoardId = _jediAppDb.ExchangeOfficeBoards.FirstOrDefault().Id
            //    };
            //    _jediAppDb.Add(aud);
            //}

            //var cad = _jediAppDb.Currencys.Where(c => (c.ShortName.ToLower()).Equals("cad")).FirstOrDefault();
            //if (cad is null)
            //{
            //    cad = new Currency
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Canadian dollar",
            //        ShortName = "CAD",
            //        Country = "Canada",
            //        BuyAt = 1,
            //        SellAt = 1,
            //        ExchangeOfficeBoardId = _jediAppDb.ExchangeOfficeBoards.FirstOrDefault().Id
            //    };
            //    _jediAppDb.Add(cad);
            //}

            //var czk = _jediAppDb.Currencys.Where(c => (c.ShortName.ToLower()).Equals("czk")).FirstOrDefault();
            //if (czk is null)
            //{
            //    czk = new Currency
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Czech koruna",
            //        ShortName = "CZK",
            //        Country = "Czech Republic",
            //        BuyAt = 1,
            //        SellAt = 1,
            //        ExchangeOfficeBoardId = _jediAppDb.ExchangeOfficeBoards.FirstOrDefault().Id
            //    };
            //    _jediAppDb.Add(czk);
            //}

            //var dkk = _jediAppDb.Currencys.Where(c => (c.ShortName.ToLower()).Equals("dkk")).FirstOrDefault();
            //if (dkk is null)
            //{
            //    dkk = new Currency
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Danish krone",
            //        ShortName = "DKK",
            //        Country = "Denmark",
            //        BuyAt = 1,
            //        SellAt = 1,
            //        ExchangeOfficeBoardId = _jediAppDb.ExchangeOfficeBoards.FirstOrDefault().Id
            //    };
            //    _jediAppDb.Add(dkk);
            //}

            //var huf = _jediAppDb.Currencys.Where(c => (c.ShortName.ToLower()).Equals("huf")).FirstOrDefault();
            //if (huf is null)
            //{
            //    huf = new Currency
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Hungarian forint",
            //        ShortName = "HUF",
            //        Country = "Hungary",
            //        BuyAt = 1,
            //        SellAt = 1,
            //        ExchangeOfficeBoardId = _jediAppDb.ExchangeOfficeBoards.FirstOrDefault().Id
            //    };
            //    _jediAppDb.Add(huf);
            //}

            //var jpy = _jediAppDb.Currencys.Where(c => (c.ShortName.ToLower()).Equals("jpy")).FirstOrDefault();
            //if (jpy is null)
            //{
            //    jpy = new Currency
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Japanese yen",
            //        ShortName = "JPY",
            //        Country = "Japan",
            //        BuyAt = 1,
            //        SellAt = 1,
            //        ExchangeOfficeBoardId = _jediAppDb.ExchangeOfficeBoards.FirstOrDefault().Id
            //    };
            //    _jediAppDb.Add(jpy);
            //}

            //var nok = _jediAppDb.Currencys.Where(c => (c.ShortName.ToLower()).Equals("nok")).FirstOrDefault();
            //if (nok is null)
            //{
            //    nok = new Currency
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Norwegian krone",
            //        ShortName = "NOK",
            //        Country = "Norway",
            //        BuyAt = 1,
            //        SellAt = 1,
            //        ExchangeOfficeBoardId = _jediAppDb.ExchangeOfficeBoards.FirstOrDefault().Id
            //    };
            //    _jediAppDb.Add(nok);
            //}

            //var sek = _jediAppDb.Currencys.Where(c => (c.ShortName.ToLower()).Equals("sek")).FirstOrDefault();
            //if (sek is null)
            //{
            //    sek = new Currency
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Swedish krona",
            //        ShortName = "SEK",
            //        Country = "Sweden",
            //        BuyAt = 1,
            //        SellAt = 1,
            //        ExchangeOfficeBoardId = _jediAppDb.ExchangeOfficeBoards.FirstOrDefault().Id
            //    };
            //    _jediAppDb.Add(sek);
            //}

            //end tmp
            _jediAppDb.SaveChanges();

            return _jediAppDb.Currencys.OrderBy(c => c.ShortName).ToList();

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
                //currency.Fluctuation = currencyToEdit.Fluctuation;

                //_jediAppDb.SaveChangesAsync();
                _jediAppDb.SaveChanges();
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
                var currency = GetCurrencyById(id);
                _jediAppDb.Remove(currency);
                //_jediAppDb.SaveChangesAsync();
                _jediAppDb.SaveChanges();

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}