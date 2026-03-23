using VistulaExchange.Database.Domain;

namespace VistulaExchange.Services.Services.Interfaces
{
    public interface IExchangeOfficeService
    {
        List<ExchangeOffice> GetAllExchangeOffices();
        ExchangeOffice GetExchangeOfficeById(Guid id);
        bool UpdateExchangeOffice(Guid id, ExchangeOffice exchangeOfficeToEdit);

    }
}