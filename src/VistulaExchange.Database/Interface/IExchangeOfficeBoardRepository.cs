using VistulaExchange.Database.Domain;

namespace VistulaExchange.Database.Interface
{
    public interface IExchangeOfficeBoardRepository
    {
        Currency AddCurrency(Currency currency);
        Currency UpdateCurrency(Currency currency);
        List<Currency> GetAllCurrencies();
        Currency GetCurrencyById(Guid id);
        List<Currency> BrowseCurrency(string query);
        bool UpdateCurrency(Guid id, Currency currencyToEdit);
        bool DeleteCurrency(Guid id);
    }
}