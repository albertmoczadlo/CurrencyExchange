using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Services.Services.Interfaces;

namespace JediApp.Services.Services.Service
{
    public class ExchangeOfficeBoardService : IExchangeOfficeBoardService
    {
        private readonly IExchangeOfficeBoardRepository _exchangeOfficeBoardRepository;

        public ExchangeOfficeBoardService(IExchangeOfficeBoardRepository exchangeOfficeBoardRepository)
        {
            _exchangeOfficeBoardRepository = exchangeOfficeBoardRepository;
        }
        public Currency AddCurrency(Currency currency)
        {
            if (IfShortNameIsUnique(currency)) //because ShortName is unique
            {
                return _exchangeOfficeBoardRepository.AddCurrency(currency);
            }
            else
            {
                return _exchangeOfficeBoardRepository.UpdateCurrency(currency);
            }
        }

        public List<Currency> GetAllCurrencies()
        {
            return _exchangeOfficeBoardRepository.GetAllCurrencies();
        }

        public Currency GetCurrencyById(Guid id)
        {
            return _exchangeOfficeBoardRepository.GetCurrencyById(id);
        }

        public List<Currency> BrowseCurrency(string query)
        {
            return _exchangeOfficeBoardRepository.BrowseCurrency(query);
        }

        public bool UpdateCurrency(Guid id, Currency currencyToEdit)
        {
            var currnecy = GetAllCurrencies().FirstOrDefault(c => c.Id == id);

            if (currnecy == null)
            {
                return false;
            }
            return _exchangeOfficeBoardRepository.UpdateCurrency(id, currencyToEdit);
        }

        public bool DeleteCurrencyByShortName(string shortName)
        {
            var currnecy = GetAllCurrencies().FirstOrDefault(c => c.ShortName.ToLowerInvariant().Equals(shortName.ToLowerInvariant()));

            if (currnecy == null)
            {
                return false;
            }
            return _exchangeOfficeBoardRepository.DeleteCurrency(currnecy.Id);
        }

        public bool IfShortNameIsUnique(Currency currency)
        {
            foreach (var _currency in GetAllCurrencies())
            {
                if (currency.ShortName.Equals(_currency.ShortName))
                    return false;
            }
            return true;
        }



    }
}
