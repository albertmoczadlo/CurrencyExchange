using VistulaExchange.Database.Domain;

namespace VistulaExchange.Database.Interface
{
    public interface IExchangeOfficeRepository
    {
        List<ExchangeOffice> GetAllExchangeOffices();
        ExchangeOffice GetExchangeOfficeById(Guid id);
        bool UpdateExchangeOffice(Guid id, ExchangeOffice exchangeOfficeToEdit);
    }
}