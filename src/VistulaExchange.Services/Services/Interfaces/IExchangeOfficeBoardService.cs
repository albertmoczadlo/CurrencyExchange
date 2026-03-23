using VistulaExchange.Database.Domain;

namespace VistulaExchange.Services.Services.Interfaces
{
    public interface IExchangeOfficeBoardService
    {
        Currency AddCurrency(Currency currency);
        List<Currency> GetAllCurrencies();
        Currency GetCurrencyById(Guid id);
        bool UpdateCurrency(Guid id, Currency currency);
        bool DeleteCurrencyByShortName(string shortName);
        List<Currency> BrowseCurrency(string query);
    }
}